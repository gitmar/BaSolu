using GxShared.GxDtos;
using GxShared.GxGuards;
using GxShared.Helpers;
using GxShared.Helpers.CrudHelpers;
using GxShared.Interfaces;
using GxShared.Sess;
// Infrastructure/Components/CrudGridBase.cs
using Microsoft.AspNetCore.Components;

using Newtonsoft.Json;

namespace GxTie.Components.Uifrags
{
    public abstract class MultiLevelCrudBase : ComponentBase
    {
        protected readonly IPendingChangesGuard Guard;

        protected readonly Dictionary<EntityLevel, EntityEditState> _editStates = new();
        protected readonly Dictionary<(EntityLevel level, Guid rowguid), RowState> _rowStates = new();
        protected readonly Dictionary<(EntityLevel level, Guid rowguid), PendingOpType> _rowPendingOpTypeByRow = new();
        protected readonly Dictionary<(EntityLevel level, Guid rowguid), PendingOpInfo> _rowOpInfoByRow = new();
        protected readonly Dictionary<Guid, Guid> _pendingOpIdsByRow = new();
        protected bool IsAnyRowEditing
            => _rowStates.Values.Any(s => s != RowState.Default);

        protected bool IsRowEditing(Guid rowguid)
            => _rowStates.Any(kv => kv.Key.rowguid == rowguid && kv.Value != RowState.Default);

        protected readonly Dictionary<Guid, bool> _isLightBg = new();

        protected List<PlngenDto> PlanItems = new();
        protected List<RubvarDto> RubItems = new();
        protected List<RubfmtDto> FmtItems = new();
        protected List<RubhieDto> HieItems = new();
        protected List<RubpstDto> PstItems = new();
        protected List<TierspDto> TieItems = new();
        protected List<ActsaieDto> ActItems = new();
        protected List<ActdetDto> AdtItems = new();
        protected List<ResdonDto> ResItems = new();
        protected List<ResdetDto> RdtItems = new();
        protected List<ResbroDto> BroItems = new();
        protected object? _draftPlan { get; set; }
        protected object? _draftRub { get; set; }
        protected object? _draftFmt { get; set; }
        protected object? _draftHie { get; set; }
        protected object? _draftPst { get; set; }
        protected object? _draftTie { get; set; }
        protected object? _draftAct { get; set; }
        protected object? _draftAdt { get; set; }
        protected object? _draftRes { get; set; }
        protected object? _draftRdt { get; set; }
        protected object? _draftBro { get; set; }
        //protected object? _draftGtb { get; set; }

        protected MultiLevelCrudBase(IPendingChangesGuard guard)
        {
            Guard = guard;
        }

        protected override void OnParametersSet()
        {
            SubscribeToGuard();
        }

        protected abstract void SubscribeToGuard();
        protected abstract void AddToLocalCollection(EntityLevel level, object entity);
        protected abstract void RemoveFromLocalCollection(EntityLevel level, object entity);
        protected abstract void RollbackPendingState(EntityLevel level, object entity, bool isNew);
        protected abstract void ReplaceInLocalCollection(EntityLevel level, object entity);
        protected abstract void CopyDraftToGridItem(EntityLevel level, object entity);
        protected abstract Task ConfirmAdd(EntityLevel level, object entity, bool isConfirm);
        protected abstract Task ConfirmEdit(EntityLevel level, object entity, bool isConfirm);
        //protected abstract Task ConfirmCancel(EntityLevel level, object entity);
        protected abstract Task ConfirmDelete(EntityLevel level, object entity, bool isConfirm);
        protected abstract Task CancelAdd(EntityLevel level, object entity);
        protected abstract Task CancelEdit(EntityLevel level, object entity);
        protected abstract Task CancelDelete(EntityLevel level, object entity);
        protected abstract void RestoreOriginalGridItem(EntityLevel level, object entity);
        protected abstract void FinalizeConfirmedState(EntityLevel level, object entity, string message);
        //protected abstract void EndEdit(EntityLevel level);
        protected virtual void OnEntitySaved(EntityLevel level, object entity)
        {
        }
        protected virtual bool Validate(EntityLevel level, object entity) => true;
        protected T DeepClone<T>(T entity)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(entity, settings);
            return JsonConvert.DeserializeObject<T>(json, settings)!;
        }
        protected int NextSeq<T>(IEnumerable<T> source, Func<T, int?> selector)
        {
            var maxSeq = source.Any() ? source.Max(selector) ?? 0 : 0;
            return maxSeq + 1;
        }
        protected PendingOpType GetPendingOpType(EntityLevel level, Guid rowguid)
            => _rowPendingOpTypeByRow.TryGetValue((level, rowguid), out var value)
                ? value
                : PendingOpType.Update;
        protected void SetPendingOpType(EntityLevel level, Guid rowguid, PendingOpType op)
            => _rowPendingOpTypeByRow[(level, rowguid)] = op;
        protected void SetOpInfo(EntityLevel level, Guid rowguid, PendingOpInfo info)
            => _rowOpInfoByRow[(level, rowguid)] = info;
        protected PendingOpInfo? GetOpInfo(EntityLevel level, Guid rowguid)
            => _rowOpInfoByRow.TryGetValue((level, rowguid), out var value)
                ? value
                : null;
        protected virtual string GetEntitySetName(EntityLevel level)
        {
            return level switch
            {
                EntityLevel.Plan => "Plngens",
                EntityLevel.Rub => "Rubvars",
                EntityLevel.Fmt => "Rubfmts",
                EntityLevel.Hie => "Rubhies",
                EntityLevel.Pst => "Rubpsts",
                EntityLevel.Tie => "Tiersps",
                EntityLevel.Act => "Actsaies",
                EntityLevel.Adt => "Actdets",
                EntityLevel.Res => "Resdons",
                EntityLevel.Rdt => "Resdets",
                EntityLevel.Bro => "Resbros",
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
            };
        }
        protected void RemoveByRowguid(EntityLevel level, Guid rowguid)
        {
            switch (level)
            {
                case EntityLevel.Plan:
                    PlanItems.RemoveAll(x => x.Rowguid == rowguid);
                    break;
                case EntityLevel.Rub:
                    RubItems.RemoveAll(x => x.Rowguid == rowguid);
                    break;
                case EntityLevel.Fmt:
                    FmtItems.RemoveAll(x => x.Rowguid == rowguid);
                    break;
                case EntityLevel.Hie:
                    HieItems.RemoveAll(x => x.Rowguid == rowguid);
                    break;
                case EntityLevel.Pst:
                    PstItems.RemoveAll(x => x.Rowguid == rowguid);
                    break;
                case EntityLevel.Tie:
                    TieItems.RemoveAll(x => x.Rowguid == rowguid);
                    break;
                case EntityLevel.Act:
                    ActItems.RemoveAll(x => x.Rowguid == rowguid);
                    break;
                case EntityLevel.Adt:
                    AdtItems.RemoveAll(x => x.Rowguid == rowguid);
                    break;
                case EntityLevel.Res:
                    ResItems.RemoveAll(x => x.Rowguid == rowguid);
                    break;
                case EntityLevel.Rdt:
                    RdtItems.RemoveAll(x => x.Rowguid == rowguid);
                    break;
                case EntityLevel.Bro:
                    BroItems.RemoveAll(x => x.Rowguid == rowguid);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level));
            }
            EndRowEdit(level);
        }
        protected EntityEditState GetEditState(EntityLevel level)
        {
            if (!_editStates.TryGetValue(level, out var state))
            {
                state = new EntityEditState();
                _editStates[level] = state;
            }
            Console.WriteLine($"State : {state}");
            return state;
        }
        protected void ClearEditState(EntityLevel level, Guid rowguid)
        {
            _editStates[level] = new EntityEditState
            {
                IsAdd = false,
                IsEdit = false,
                AddRowguid = null,
                EditRowguid = null,
                DeleteRowguid = null
            };
            Console.WriteLine("editstate = edit false");
        }
        protected void SetEditState(EntityLevel level, bool isAdd, bool isEdit, Guid? rowguid)
        {
            var s = GetEditState(level);
            s.IsAdd = isAdd;
            s.IsEdit = isEdit;
            s.AddRowguid = isAdd ? rowguid : null;
            s.EditRowguid = isEdit ? rowguid : null;
            s.DeleteRowguid = null;
        }
        protected Guid GetRenderKey(EntityLevel level)
        {
            if (!_renderKeys.TryGetValue(level, out var key))
            {
                key = Guid.NewGuid();
                _renderKeys[level] = key;
            }
            return key;
        }
        protected void BumpRenderKey(EntityLevel level)
        {
            _renderKeys[level] = Guid.NewGuid();
        }
        protected virtual void OnLocalCollectionMutated(EntityLevel level, CollectionMutation mutation, object entity)
        {
        }
        protected void EndRowEdit(EntityLevel level)
        {
            var es = GetEditState(level);
            es.IsAdd = false;
            es.IsEdit = false;
            es.AddRowguid = null;
            es.EditRowguid = null;
            es.DeleteRowguid = null;
        }
        protected string GetRowCssFor(EntityLevel level, Guid rowguid)
        {
            var state = GetRowState(level, rowguid);
            var isLight = IsLightBackground(rowguid);
            var isActiveEdit = GetRowState(level, rowguid) is RowState.AddPending or RowState.EditPending;
            var isOtherRowEditing = IsAnyRowEditing && !isActiveEdit;
   
            return GetRowCss(state, isLight, rowguid, isActiveEdit, isOtherRowEditing);
        }
        protected string GetPlanRowClass(PlngenDto pln) => GetRowCssFor(EntityLevel.Plan, pln.Rowguid);
        protected string GetRubRowClass(RubvarDto rub) => GetRowCssFor(EntityLevel.Rub, rub.Rowguid);
        protected string GetFmtRowClass(RubfmtDto fmt) => GetRowCssFor(EntityLevel.Fmt, fmt.Rowguid);
        protected string GetHieRowClass(RubhieDto hie) => GetRowCssFor(EntityLevel.Hie, hie.Rowguid);
        protected string GetPstRowClass(RubpstDto pst) => GetRowCssFor(EntityLevel.Pst, pst.Rowguid);
        protected string GetTieRowClass(TierspDto tie) => GetRowCssFor(EntityLevel.Tie, tie.Rowguid);
        protected string GetActRowClass(ActsaieDto act) => GetRowCssFor(EntityLevel.Act, act.Rowguid);
        protected string GetAdtRowClass(ActdetDto adt) => GetRowCssFor(EntityLevel.Adt, adt.Rowguid);
        protected string GetResRowClass(ResdonDto res) => GetRowCssFor(EntityLevel.Res, res.Rowguid);
        protected string GetRdtRowClass(ResdetDto rdt) => GetRowCssFor(EntityLevel.Rdt, rdt.Rowguid);
        protected string GetBroRowClass(ResbroDto bro) => GetRowCssFor(EntityLevel.Bro, bro.Rowguid);
        protected string GetFixRowClass(GstablDto fix) => GetRowCssFor(EntityLevel.Plan, fix.Rowguid);
        protected string GetChlRowClass(GstablDto chl) => GetRowCssFor(EntityLevel.Plan, chl.Rowguid);
        
        protected RowState GetRowState(EntityLevel level, Guid rowguid)
        {
            var state = _rowStates.TryGetValue((level, rowguid), out var value)
       ? value
       : RowState.Default;

            return state;
        }
        protected void SetRowState(EntityLevel level, Guid rowguid, RowState state)
        {
            Console.WriteLine($"row state IS SET : {state}");
            _rowStates[(level, rowguid)] = state;
        }
        // --- shared start-edit entry point --- 
        protected void ResetRowState(EntityLevel level, Guid rowguid)
            => _rowStates.Remove((level, rowguid));
        protected bool IsLightBackground(Guid rowguid)
            => _isLightBg.TryGetValue(rowguid, out var value) && value;
        protected string GetRowCss(RowState state, bool isLight, Guid rowguid, bool isActiveEdit, bool isOtherRowEditing)
        {
            var css = new List<string>();

            if (isLight)
                css.Add("bg-light");

            if (isActiveEdit)
            {
                css.Add("row-active-edit");
                return string.Join(" ", css);
            }

            if (isOtherRowEditing)
                css.AddRange(new[] { "row-readonly", "opacity-65", "cursor-not-allowed" });

            switch (state)
            {
                case RowState.AddPending:
                case RowState.EditPending:
                    css.Add("border-start border-info border-3");
                    break;
                case RowState.DeletePending:
                    css.Add("bg-danger-subtle");
                    break;
                case RowState.Locked:
                    css.Add("bg-secondary-subtle opacity-75");
                    break;
            }
            return string.Join(" ", css);
        }
        private readonly Dictionary<EntityLevel, Guid> _renderKeys = new();
        protected class EntityEditState
        {
            public bool IsAdd { get; set; }
            public bool IsEdit { get; set; }
            public Guid? AddRowguid { get; set; }
            public Guid? EditRowguid { get; set; }
            public Guid? DeleteRowguid { get; set; }
        }
        protected enum CollectionMutation { Added, Removed, Replaced }

        // Fired after PlanItems/RubItems/FmtItems (etc.) have been mutated,
        // so a derived component can keep its own source-of-truth lists
        // (orgPlns, curPlan.Rubvars, curRubr.Rubfmts, ...) in sync.
    }
}