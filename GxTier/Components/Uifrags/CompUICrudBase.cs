using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Cryptography;

using BlazorBootstrap;

//using GxPilo.Components.Plans;

using GxShared.GxDtos;
using GxShared.Helpers;
using GxShared.Helpers.CrudHelpers;
using GxShared.Interfaces;
using GxShared.Sess;

using Newtonsoft.Json;

namespace GxTie.Components.Uifrags
{
    public abstract class CompUICrudBase : MultiLevelCrudBase
    {
        protected readonly Dictionary<Guid, PendingOpType> _rowPendingOpTypeByRow = new();
        
        protected readonly Dictionary<Guid, TierspDto> _edTieOriginals = new();
        protected readonly Dictionary<Guid, TiewelDto> _edTiwOriginals = new();
        protected readonly Dictionary<Guid, TieaflDto> _edAflOriginals = new();
        protected readonly Dictionary<Guid, TiwaflDto> _edWflOriginals = new();
        protected readonly Dictionary<Guid, ActsaieDto> _edActOriginals = new();
        protected readonly Dictionary<Guid, ActdetDto> _edAdtOriginals = new();
        protected readonly Dictionary<Guid, ResdonDto> _edResOriginals = new();
        protected readonly Dictionary<Guid, ResdetDto> _edRdtOriginals = new();
        protected readonly Dictionary<Guid, ResbroDto> _edBroOriginals = new();
        protected CompUICrudBase(IPendingChangesGuard guard) : base(guard)
        {

        }
        //protected abstract void ClearAddRow(EntityLevel level, Guid rowguid);
        //protected abstract void ClearEditRow(EntityLevel level, Guid rowguid);
        protected override async Task ConfirmAdd(EntityLevel level, object draft, bool isConfirm)
        {

            //var rowguid = EntityKeyHelper.GetRowguid(draft!); // get it from the draft
            await UnifiedAddAction(level, draft, PendingOpType.Insert, isConfirm);
            //var soe = curRubr.Rubfmts.ToList();
            EndRowEdit(level); //
            BumpRenderKey(level);                 // <-- add this
            await InvokeAsync(StateHasChanged);
        }
        protected override async Task ConfirmEdit(EntityLevel level, object draft, bool isConfirm)
        {
            if (draft is null) return;
            await UnifiedEditAction(level, draft, PendingOpType.Insert, isConfirm);
            EndRowEdit(level); //
            BumpRenderKey(level);                 // <-- add this
            await InvokeAsync(StateHasChanged);
        }   
        protected override async Task ConfirmDelete(EntityLevel level, object entity, bool isConfirm)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);
            await UnifiedDeleteAction(level, entity, isConfirm);
            ////ClearAddRow(level, rowguid);
            BumpRenderKey(level);                 // <-- add this
            await InvokeAsync(StateHasChanged);
        }
        protected override async Task CancelAdd(EntityLevel level, object entity)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);
            await UnifiedCancelAction(level, entity, PendingOpType.Insert);
            ////ClearAddRow(level, rowguid);
            EndRowEdit(level);
            BumpRenderKey(level);                 // <-- add this
            await InvokeAsync(StateHasChanged);
        }
        protected override async Task CancelEdit(EntityLevel level, object entity)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);
            Console.WriteLine($"CancelEdit rowguid = {rowguid}");
            //await UnifiedCancelAction(level, entity, PendingOpType.Update);
            await UnifiedCancelAction(level, entity, PendingOpType.Update);
            ////ClearEditRow(level, rowguid);
            //await ClearEditRow(level, entity);
            EndRowEdit(level);
            BumpRenderKey(level);                 // <-- add this
            await InvokeAsync(StateHasChanged);
        }
        protected override async Task CancelDelete(EntityLevel level, object entity)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);
            await UnifiedCancelAction(level, entity, PendingOpType.Delete);
            BumpRenderKey(level);                 // <-- add this
            await InvokeAsync(StateHasChanged);
        }

        // --- Unified actions (generic) ---
        protected async Task UnifiedAddAction(EntityLevel level, object entity, PendingOpType opType, bool isConfirm)
        {
            var rowguid = EntityKeyHelper.GetRowguid(entity); // trust the entity, not the caller's guid
            var isNew = GetPendingOpType(level, rowguid) == PendingOpType.Insert;
            string entitySetName = GetEntitySetName(level);
            if (!isConfirm)
            {
                if (isNew)
                    RemoveFromLocalCollection(level, entity);
                RollbackPendingState(level, entity, isNew);
                return;
            }
            CopyDraftToGridItem(level, entity);
            //CopyDraftTablToGridItem(entity, 1); // Assuming 1 is the tent for the main entity

            Guid opId;
            if (isNew)
            {
                opId = await Guard.TrackInsert(entitySetName, entity);
                ////SetPendingOpType(level, rowguid, PendingOpType.Insert);
            }
            else
            {
                var key = EntityKeyHelper.GetKey(entity, level);
                opId = await Guard.TrackUpdate(entitySetName, key, entity);
                ////SetPendingOpType(level, rowguid, PendingOpType.Update);
            }

            SetOpInfo(level, rowguid, new PendingOpInfo(opId, isNew ? PendingOpType.Insert : PendingOpType.Update));

            if (isNew)
            {
                RemoveFromLocalCollection(level, entity);
                AddToLocalCollection(level, entity);
            }

            FinalizeConfirmedState(level, entity, $"✅ {entitySetName} tracked");
        }
        protected async Task UnifiedEditAction(EntityLevel level, object entity, PendingOpType opType, bool isConfirm)
        {
            var rowguid = EntityKeyHelper.GetRowguid(entity);
            string entitySetName = GetEntitySetName(level);
            if (!isConfirm)
            {
                RollbackPendingState(level, entity, false);
                return;
            }
            if (!Validate(level,entity))
                return;

            CopyDraftToGridItem(level, entity); // Assuming 2 is the tent for the child entity

            var key = EntityKeyHelper.GetKey(entity, level);
            var opId = await Guard.TrackUpdate(entitySetName, key, entity);
            ////SetPendingOpType(level, rowguid, PendingOpType.Update);
            SetOpInfo(level, rowguid, new PendingOpInfo(opId, PendingOpType.Update));

            ReplaceInLocalCollection(level, entity);

            FinalizeConfirmedState(level, entity, $"✅ {entitySetName} tracked");
        }
        protected async Task UnifiedDeleteAction(EntityLevel level, object entity, PendingOpType opType, bool isConfirm)
        {
            var rowguid = EntityKeyHelper.GetRowguid(entity);
            string entitySetName = GetEntitySetName(level);
            if (!isConfirm)
            {
                SetRowState(level, rowguid, RowState.Default);
                return;
            }

            var key = EntityKeyHelper.GetKey(entity, level);
            var opId = await Guard.TrackDelete(entitySetName, key, rowguid);
            ////SetPendingOpType(level, rowguid, PendingOpType.Delete);
            SetOpInfo(level, rowguid, new PendingOpInfo(opId, PendingOpType.Delete));

            RemoveFromLocalCollection(level, entity);

            FinalizeConfirmedState(level, entity, $"✅ {entitySetName} deletion tracked");
        }
        protected async Task UnifiedCancelAction(EntityLevel level, object entity, PendingOpType opType)
        {
            var rowguid = EntityKeyHelper.GetRowguid(entity);
            var opInfo = GetOpInfo(level, rowguid);
            string entitySetName = GetEntitySetName(level); // You can customize this based on your entity type
            if (opInfo is not null)
                await Guard.CancelTrackAsync(opInfo.OpId, opType);

            switch (opType)
            {
                case PendingOpType.Insert:
                    //emoveByRowguid(level, rowguid);
                    RollbackPendingState(level, entity, true);
                    SetRowState(level, rowguid, RowState.Default);
                    break;

                case PendingOpType.Update:
                    //RestoreOriginalGridItem(level, entity);
                    RollbackPendingState(level, entity, false);
                    SetRowState(level, rowguid, RowState.Default);
                    break;

                case PendingOpType.Delete:
                    RollbackPendingState(level, entity, false);
                    SetRowState(level, rowguid, RowState.Default);
                    break;
            }
        }
        protected async Task UnifiedDeleteAction(EntityLevel level, object item, bool isConfirm)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(item);
            var entitySetName = GetEntitySetName(level);
            var key = MLEntityKeyHelper.GetKeyAsObject(item);
            var opId = await Guard.TrackDelete(entitySetName, key, rowguid);
            ////SetPendingOpType(level, rowguid, PendingOpType.Delete);
            SetOpInfo(level, rowguid, new PendingOpInfo(opId, PendingOpType.Delete));
            RemoveByRowguid(level, rowguid);
            SetDeleteFlags(item);
            Console.WriteLine($"ROW is removed");
            await InvokeAsync(StateHasChanged);
        }
        //protected void BeginAdd(EntityLevel level, Guid rowguid, object draft)
        //{
        //    var s = GetEditState(level);
        //    s.IsAdd = true;
        //    s.IsEdit = false;
        //    s.AddRowguid = rowguid;
        //    s.EditRowguid = null;
        //    s.DeleteRowguid = null;

        //    BeginAddRow(level, rowguid, draft);
        //    SetPendingOpType2(level, rowguid, PendingOpType.Insert);   // <- use the helper, matches GetPendingOpType's key
        //}
        // --- shared start-edit entry point ---
        protected async Task StartRowAdd<TDto>(
    EntityLevel level,
    TDto newItem,
    List<TDto> list,
    Action<TDto> setDraftField) where TDto : class
        {
            var rowguid = EntityKeyHelper.GetRowguid(newItem);
            if (rowguid == Guid.Empty) return;
            if (IsAnyRowEditing && !IsRowEditing(rowguid)) return;

            list.Insert(0, newItem);
            var draft = DeepClone(newItem);
            setDraftField(draft);

            BeginAdd(level, rowguid, draft);
            BumpRenderKey(level);                 // <-- add this
            await InvokeAsync(StateHasChanged);
        }
        protected async Task StartRowEdit<TDto>(
            EntityLevel level,
            TDto item,
            Dictionary<Guid, TDto> originalsStore,
            Action<TDto> setDraftField) where TDto : class
        {
            var rowguid = EntityKeyHelper.GetRowguid(item);
            if (rowguid == Guid.Empty) return;
            if (IsAnyRowEditing && !IsRowEditing(rowguid)) return;

            originalsStore[rowguid] = DeepClone(item);
            var draft = DeepClone(item);
            setDraftField(draft);

            BeginEdit(level, rowguid, draft);
            BumpRenderKey(level);                 // <-- add this
            await InvokeAsync(StateHasChanged);
        }
        protected async Task StartRowDelete(EntityLevel level, Guid rowguid)
        {
            if (rowguid == Guid.Empty) return;
            if (IsAnyRowEditing && !IsRowEditing(rowguid)) return;

            SetRowState(level, rowguid, RowState.DeletePending);
            BumpRenderKey(level);                 // <-- add this
            await InvokeAsync(StateHasChanged);
        }
        protected void BeginAdd(EntityLevel level, Guid rowguid, object draft)
        {
            SetDraft(level, draft);
            SetEditState(level, isAdd: true, isEdit: false, rowguid);   // <- restore this
            SetRowState(level, rowguid, RowState.AddPending);
            SetPendingOpType(level, rowguid, PendingOpType.Insert);
            SetLightBackground(rowguid, true);
        }

        protected void BeginEdit(EntityLevel level, Guid rowguid, object draft)
        {
            SetDraft(level, draft);
            SetEditState(level, isAdd: false, isEdit: true, rowguid);   // <- restore this
            SetRowState(level, rowguid, RowState.EditPending);
            SetPendingOpType(level, rowguid, PendingOpType.Update);
            SetLightBackground(rowguid, true);
        }
        protected bool IsTieEditing(TierspDto item) => IsEditing(EntityLevel.Tie, item.Rowguid);
        protected bool IsTiwEditing(TiewelDto item) => IsEditing(EntityLevel.Tiw, item.Rowguid);
        protected bool IsAflEditing(TieaflDto item) => IsEditing(EntityLevel.Afl, item.Rowguid);
        protected bool IsWflEditing(TiwaflDto item) => IsEditing(EntityLevel.Wfl, item.Rowguid);
        protected bool IsEditing(EntityLevel level, Guid rowguid)
        {
            var es = GetEditState(level);
            return (es.IsAdd || es.IsEdit) &&
                   ((es.IsAdd && es.AddRowguid == rowguid) ||
                    (es.IsEdit && es.EditRowguid == rowguid));
        }
        private void SetDeleteFlags(object item) => SetFlags(item, 0, 0, -1);
        // Generic flag setter
        private void SetFlags<T>(T entity, int xadd1, int xedt1, int xdel1)
        {
            typeof(T).GetProperty("Xadd1")?.SetValue(entity, xadd1);
            typeof(T).GetProperty("Xedt1")?.SetValue(entity, xedt1);
            typeof(T).GetProperty("Xdel1")?.SetValue(entity, xdel1);
        }
        protected bool IsLevelBusy(EntityLevel level)
        {
            var es = GetEditState(level);
            return es.IsAdd || es.IsEdit;
        }
        protected async Task SaveAllChanges()
        {
            await Guard.FlushAsync();  // 🔥 AWAIT - blocks until done
            //_messageService.Show("✅ Saved successfully", ToastType.Success);
            StateHasChanged();
        }
        protected override void RollbackPendingState(EntityLevel level, object entity, bool isNew)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);

            if (isNew)
            {
                Console.WriteLine($"NEW ROW TO REMOVE : {rowguid}");
                RemoveByRowguid(level, rowguid);
                ////ClearAddRow(level, rowguid);
            }
            //RemoveFromLocalCollection(level, entity);
            else
                RestoreOriginalGridItem(level, entity);

            ClearEditState(level, rowguid);
            SetLightBackground(rowguid, false);

            _rowStates.Remove((level, rowguid));

            // add this
            _rowPendingOpTypeByRow.Remove(rowguid);

            // if you use this too:
            _pendingOpIdsByRow.Remove(rowguid);

            //EndRowEdit();
            SetLightBackground(rowguid, false);
        }
        private void SetLightBackground(Guid rowguid, bool isLight)
        {
            _isLightBg[rowguid] = isLight;
        }
        private bool GetLightBackground(Guid rowguid)
        {
            return _isLightBg.GetValueOrDefault(rowguid, false);
        }

        protected override void AddToLocalCollection(EntityLevel level, object entity)
        {
            switch (level)
            {
                case EntityLevel.Tie:
                    if (entity is TierspDto tie) TieItems.Add(tie);
                    break;
                case EntityLevel.Tiw:
                    if (entity is TiewelDto tiw) TiwItems.Add(tiw);
                    break;
                case EntityLevel.Afl:
                    if (entity is TieaflDto afl) AflItems.Add(afl);
                    break;
                case EntityLevel.Wfl:
                    if (entity is TiwaflDto wfl) WflItems.Add(wfl);
                    break;
                case EntityLevel.Act:
                    if (entity is ActsaieDto act) ActItems.Add(act);
                    break;
                case EntityLevel.Adt:
                    if (entity is ActdetDto adt) AdtItems.Add(adt);
                    break;
                case EntityLevel.Res:
                    if (entity is ResdonDto res) ResItems.Add(res);
                    break;
                case EntityLevel.Rdt:
                    if (entity is ResdetDto rdt) RdtItems.Add(rdt);
                    break;
                case EntityLevel.Bro:
                    if (entity is ResbroDto bro) BroItems.Add(bro);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level));
            }
            OnLocalCollectionMutated(level, CollectionMutation.Added, entity);
        }
        protected override void RemoveFromLocalCollection(EntityLevel level, object entity)
        {
            switch (level)
            {
                case EntityLevel.Tie:
                    if (entity is TierspDto tie)
                        TieItems.RemoveAll(x => x.Rowguid == tie.Rowguid);
                    break;
                case EntityLevel.Tiw:
                    if (entity is TiewelDto tiw)
                        TiwItems.RemoveAll(x => x.Rowguid == tiw.Rowguid);
                    break;
                case EntityLevel.Afl:
                    if (entity is TieaflDto afl)
                        AflItems.RemoveAll(x => x.Rowguid == afl.Rowguid);
                    break;
                case EntityLevel.Wfl:
                    if (entity is TiwaflDto wfl)
                        WflItems.RemoveAll(x => x.Rowguid == wfl.Rowguid);
                    break;
                case EntityLevel.Act:
                    if (entity is ActsaieDto act)
                        ActItems.RemoveAll(x => x.Rowguid == act.Rowguid);
                    break;
                case EntityLevel.Adt:
                    if (entity is ActdetDto adt)
                        AdtItems.RemoveAll(x => x.Rowguid == adt.Rowguid);
                    break;
                case EntityLevel.Res:
                    if (entity is ResdonDto res)
                        ResItems.RemoveAll(x => x.Rowguid == res.Rowguid);
                    break;
                case EntityLevel.Rdt:
                    if (entity is ResdetDto rdt)
                        RdtItems.RemoveAll(x => x.Rowguid == rdt.Rowguid);
                    break;
                case EntityLevel.Bro:
                    if (entity is ResbroDto bro)
                        BroItems.RemoveAll(x => x.Rowguid == bro.Rowguid);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level));
            }
            OnLocalCollectionMutated(level, CollectionMutation.Removed, entity);
        }
        protected override void ReplaceInLocalCollection(EntityLevel level, object entity)
        {
            switch (level)
            {
                case EntityLevel.Tie:
                    if (entity is TierspDto tie)
                    {
                        var index = TieItems.FindIndex(x => x.Rowguid == tie.Rowguid);
                        if (index >= 0) TieItems[index] = tie;
                    }
                    break;
                case EntityLevel.Tiw:
                    if (entity is TiewelDto tiw)
                    {
                        var index = TiwItems.FindIndex(x => x.Rowguid == tiw.Rowguid);
                        if (index >= 0) TiwItems[index] = tiw;
                    }
                    break;
                case EntityLevel.Afl:
                    if (entity is TieaflDto afl)
                    {
                        var index = AflItems.FindIndex(x => x.Rowguid == afl.Rowguid);
                        if (index >= 0) AflItems[index] = afl;
                    }
                    break;
                case EntityLevel.Wfl:
                    if (entity is TiwaflDto wfl)
                    {
                        var index = WflItems.FindIndex(x => x.Rowguid == wfl.Rowguid);
                        if (index >= 0) WflItems[index] = wfl;
                    }
                    break;
                case EntityLevel.Act:
                    if (entity is ActsaieDto act)
                    {
                        var index = ActItems.FindIndex(x => x.Rowguid == act.Rowguid);
                        if (index >= 0) ActItems[index] = act;
                    }
                    break;
                case EntityLevel.Adt:
                    if (entity is ActdetDto adt)
                    {
                        var index = AdtItems.FindIndex(x => x.Rowguid == adt.Rowguid);
                        if (index >= 0) AdtItems[index] = adt;
                    }
                    break;
                case EntityLevel.Res:
                    if (entity is ResdonDto res)
                    {
                        var index = ResItems.FindIndex(x => x.Rowguid == res.Rowguid);
                        if (index >= 0) ResItems[index] = res;
                    }
                    break;
                case EntityLevel.Rdt:
                    if (entity is ResdetDto rdt)
                    {
                        var index = RdtItems.FindIndex(x => x.Rowguid == rdt.Rowguid);
                        if (index >= 0) RdtItems[index] = rdt;
                    }
                    break;
                case EntityLevel.Bro:
                    if (entity is ResbroDto bro)
                    {
                        var index = BroItems.FindIndex(x => x.Rowguid == bro.Rowguid);
                        if (index >= 0) BroItems[index] = bro;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level));
            }
            OnLocalCollectionMutated(level, CollectionMutation.Replaced, entity);
        }

protected void CopyPropertiesWithRules<T>(T target, T source)
    {
        if (target == null || source == null) return;

        // Manual blacklist for audit/system fields
        var blacklist = new HashSet<string>
    {
        "Datc", "Dati", "Datu", "Demb", "Dinscr",
        "Rowguid", "Xrowguid"
    };

        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in props)
        {
            // Skip if property is blacklisted
            if (blacklist.Contains(prop.Name))
                continue;

            // Skip if property has [Key] attribute
            if (prop.GetCustomAttribute<KeyAttribute>() != null)
                continue;

            // Skip if property has [JsonIgnore] attribute
            if (prop.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                continue;

            // Skip if property is not writable
            if (!prop.CanWrite) continue;

            var value = prop.GetValue(source);

            // Commit only non-null values
            if (value != null)
            {
                prop.SetValue(target, value);
            }
        }
    }

    protected override void CopyDraftToGridItem(EntityLevel level, object entity)
    {
        switch (level)
        {
            case EntityLevel.Tie:
                if (entity is TierspDto tie && _draftTie is TierspDto draftTie)
                {
                    CopyPropertiesWithRules(tie, draftTie);
                }
                break;
            case EntityLevel.Tiw:
                if (entity is TiewelDto tiw && _draftTiw is TiewelDto draftTiw)
                {
                    CopyPropertiesWithRules(tiw, draftTiw);
                }
                break;
            case EntityLevel.Act:
                if (entity is ActsaieDto act && _draftAct is ActsaieDto draftAct)
                {
                    CopyPropertiesWithRules(act, draftAct);
                }
                break;
            case EntityLevel.Adt:
                if (entity is ActdetDto adt && _draftAdt is ActdetDto draftAdt)
                {
                    CopyPropertiesWithRules(adt, draftAdt);
                }
                break;
            case EntityLevel.Res:
                if (entity is ResdonDto res && _draftRes is ResdonDto draftRes)
                {
                    CopyPropertiesWithRules(res, draftRes);
                }
                break;
            case EntityLevel.Rdt:
                if (entity is ResdetDto rdt && _draftRdt is ResdetDto draftRdt)
                {
                    CopyPropertiesWithRules(rdt, draftRdt);
                }
                break;
            case EntityLevel.Bro:
                if (entity is ResbroDto bro && _draftBro is ResbroDto draftBro)
                {
                    CopyPropertiesWithRules(bro, draftBro);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(level));
        }
    }

    protected override void RestoreOriginalGridItem(EntityLevel level, object entity)
    {
        switch (level)
        {
            case EntityLevel.Tie:
                if (entity is TiewelDto tie && _draftTie is TiewelDto draftTie)
                {
                    CopyPropertiesWithRules(tie, draftTie);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(level));
        }
    }

    //committing drafts
    protected void CommitTieDraft(TierspDto target, TierspDto source)
        {
            // Manual blacklist for audit/system fields
            var blacklist = new HashSet<string>
            {
                nameof(TierspDto.Datc),
                nameof(TierspDto.Dati),
                nameof(TierspDto.Datu),
                nameof(TierspDto.Xrowguid)
            };
            var props = typeof(TierspDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                // Skip if property is blacklisted
                if (blacklist.Contains(prop.Name))
                    continue;
                // Skip if property has [Key] attribute
                if (prop.GetCustomAttribute<KeyAttribute>() != null)
                    continue;
                // Skip if property has [JsonIgnore] attribute
                if (prop.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                    continue;
                // Skip if property is not writable
                if (!prop.CanWrite) continue;
                var value = prop.GetValue(source);
                // Commit only non-null values
                if (value != null)
                {
                    prop.SetValue(target, value);
                }
            }
        }
     
        protected void CommitTiwDraft(TiewelDto target, TiewelDto source)
        {
            // Manual blacklist for audit/system fields
            var blacklist = new HashSet<string>
            {
                nameof(TiewelDto.Datc),
                nameof(TiewelDto.Dati),
                nameof(TiewelDto.Datu),
                nameof(TiewelDto.Xrowguid)
            };
            var props = typeof(TiewelDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                // Skip if property is blacklisted
                if (blacklist.Contains(prop.Name))
                    continue;
                // Skip if property has [Key] attribute
                if (prop.GetCustomAttribute<KeyAttribute>() != null)
                    continue;
                // Skip if property has [JsonIgnore] attribute
                if (prop.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                    continue;
                // Skip if property is not writable
                if (!prop.CanWrite) continue;
                var value = prop.GetValue(source);
                // Commit only non-null values
                if (value != null)
                {
                    prop.SetValue(target, value);
                }
            }
        }

        protected void CommitAflDraft(TieaflDto target, TieaflDto source)
        {
            // Manual blacklist for audit/system fields
            var blacklist = new HashSet<string>
            {
                nameof(TieaflDto.Datc),
            };
            var props = typeof(TieaflDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                // Skip if property is blacklisted
                if (blacklist.Contains(prop.Name))
                    continue;
                // Skip if property has [Key] attribute
                if (prop.GetCustomAttribute<KeyAttribute>() != null)
                    continue;
                // Skip if property has [JsonIgnore] attribute
                if (prop.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                    continue;
                // Skip if property is not writable
                if (!prop.CanWrite) continue;
                var value = prop.GetValue(source);
                // Commit only non-null values
                if (value != null)
                {
                    prop.SetValue(target, value);
                }
            }
        }
        protected void CommitWflDraft(TiwaflDto target, TiwaflDto source)
        {
            // Manual blacklist for audit/system fields
            var blacklist = new HashSet<string>
            {
                nameof(TiwaflDto.Datc),
            };
            var props = typeof(TiwaflDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                // Skip if property is blacklisted
                if (blacklist.Contains(prop.Name))
                    continue;
                // Skip if property has [Key] attribute
                if (prop.GetCustomAttribute<KeyAttribute>() != null)
                    continue;
                // Skip if property has [JsonIgnore] attribute
                if (prop.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                    continue;
                // Skip if property is not writable
                if (!prop.CanWrite) continue;
                var value = prop.GetValue(source);
                // Commit only non-null values
                if (value != null)
                {
                    prop.SetValue(target, value);
                }
            }
        }
        protected void CommitActDraft(ActsaieDto target, ActsaieDto source)
        {
            // Manual blacklist for audit/system fields
            var blacklist = new HashSet<string>
            {
                nameof(ActsaieDto.Datc),
            };
            var props = typeof(ActsaieDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                // Skip if property is blacklisted
                if (blacklist.Contains(prop.Name))
                    continue;
                // Skip if property has [Key] attribute
                if (prop.GetCustomAttribute<KeyAttribute>() != null)
                    continue;
                // Skip if property has [JsonIgnore] attribute
                if (prop.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                    continue;
                // Skip if property is not writable
                if (!prop.CanWrite) continue;
                var value = prop.GetValue(source);
                // Commit only non-null values
                if (value != null)
                {
                    prop.SetValue(target, value);
                }
            }
        }
        protected void CommitAdtDraft(ActdetDto target, ActdetDto source)
        {
            // Manual blacklist for audit/system fields
            var blacklist = new HashSet<string>
            {
                nameof(ActdetDto.Datc),
            };
            var props = typeof(ActdetDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                // Skip if property is blacklisted
                if (blacklist.Contains(prop.Name))
                    continue;
                // Skip if property has [Key] attribute
                if (prop.GetCustomAttribute<KeyAttribute>() != null)
                    continue;
                // Skip if property has [JsonIgnore] attribute
                if (prop.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                    continue;
                // Skip if property is not writable
                if (!prop.CanWrite) continue;
                var value = prop.GetValue(source);
                // Commit only non-null values
                if (value != null)
                {
                    prop.SetValue(target, value);
                }
            }
        }
        protected void CommitResDraft(ResdonDto target, ResdonDto source)
        {
            // Manual blacklist for audit/system fields
            var blacklist = new HashSet<string>
            {
                nameof(ResdonDto.Datc),
            };
            var props = typeof(ResdonDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                // Skip if property is blacklisted
                if (blacklist.Contains(prop.Name))
                    continue;
                // Skip if property has [Key] attribute
                if (prop.GetCustomAttribute<KeyAttribute>() != null)
                    continue;
                // Skip if property has [JsonIgnore] attribute
                if (prop.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                    continue;
                // Skip if property is not writable
                if (!prop.CanWrite) continue;
                var value = prop.GetValue(source);
                // Commit only non-null values
                if (value != null)
                {
                    prop.SetValue(target, value);
                }
            }
        }
        protected void CommitRdtDraft(ResdetDto target, ResdetDto source)
        {
            // Manual blacklist for audit/system fields
            var blacklist = new HashSet<string>
            {
                nameof(ResdetDto.Datc),
            };
            var props = typeof(ResdetDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                // Skip if property is blacklisted
                if (blacklist.Contains(prop.Name))
                    continue;
                // Skip if property has [Key] attribute
                if (prop.GetCustomAttribute<KeyAttribute>() != null)
                    continue;
                // Skip if property has [JsonIgnore] attribute
                if (prop.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                    continue;
                // Skip if property is not writable
                if (!prop.CanWrite) continue;
                var value = prop.GetValue(source);
                // Commit only non-null values
                if (value != null)
                {
                    prop.SetValue(target, value);
                }
            }
        }
        protected void CommitBroDraft(ResbroDto target, ResbroDto source)
        {
            // Manual blacklist for audit/system fields
            var blacklist = new HashSet<string>
            {
                nameof(ResbroDto.Datc),
            };
            var props = typeof(ResbroDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                // Skip if property is blacklisted
                if (blacklist.Contains(prop.Name))
                    continue;
                // Skip if property has [Key] attribute
                if (prop.GetCustomAttribute<KeyAttribute>() != null)
                    continue;
                // Skip if property has [JsonIgnore] attribute
                if (prop.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                    continue;
                // Skip if property is not writable
                if (!prop.CanWrite) continue;
                var value = prop.GetValue(source);
                // Commit only non-null values
                if (value != null)
                {
                    prop.SetValue(target, value);
                }
            }
        }
        //divers settings
        protected object? GetDraft(EntityLevel level)
        {
            return level switch
            {
                EntityLevel.Tiw => _draftTiw,
                EntityLevel.Tie => _draftTie,
                EntityLevel.Afl => _draftAfl,
                EntityLevel.Wfl => _draftWfl,
                _ => throw new ArgumentOutOfRangeException(nameof(level))
            };
        }
        protected void SetDraft(EntityLevel level, object? draft)
        {
            switch (level)
            {
                case EntityLevel.Tie:
                    _draftTie = draft;
                    break;
                case EntityLevel.Tiw:
                    _draftTiw = draft;
                    break;
                case EntityLevel.Afl:
                    _draftAfl = draft;
                    break;
                case EntityLevel.Wfl:
                    _draftWfl = draft;
                    break;
                case EntityLevel.Act:
                    _draftAct = draft;
                    break;
                case EntityLevel.Adt:
                    _draftAdt = draft;
                    break;
                case EntityLevel.Res:
                    _draftRes = draft;
                    break;
                case EntityLevel.Rdt:
                    _draftRdt = draft;
                    break;
                case EntityLevel.Bro:
                    _draftBro = draft;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level));
            }
        }
        protected override void FinalizeConfirmedState(EntityLevel level, object entity, string message)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);

            ResetRowState(level, rowguid);

            var s = GetEditState(level);
            s.IsAdd = false;
            s.IsEdit = false;
            s.AddRowguid = null;
            s.EditRowguid = null;
            s.DeleteRowguid = null;

            EndRowEdit(level); //
            StateHasChanged();
        }
        protected static void ReplaceByRowguid<T>(List<T> items, T entity, Func<T, Guid> getRowguid)
        {
            var rowguid = getRowguid(entity);
            var index = items.FindIndex(x => getRowguid(x) == rowguid);
            if (index >= 0)
                items[index] = entity;
        }
    }
}
