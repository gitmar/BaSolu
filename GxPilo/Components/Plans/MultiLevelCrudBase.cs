using GxShared.GxDtos;
using GxShared.GxGuards;
using GxShared.Helpers;
using GxShared.Helpers.CrudHelpers;
using GxShared.Interfaces;
using GxShared.Sess;
// Infrastructure/Components/CrudGridBase.cs
using Microsoft.AspNetCore.Components;

using Newtonsoft.Json;

namespace GxPilo.Components.Plans
{
    public abstract class MultiLevelCrudBase : ComponentBase
    {
        protected readonly IPendingChangesGuard Guard;

        protected readonly Dictionary<(EntityLevel level, Guid rowguid), RowState> _rowStates = new();
        protected readonly Dictionary<(EntityLevel level, Guid rowguid), PendingOpType> _rowPendingOpTypeByRow = new();
        protected readonly Dictionary<(EntityLevel level, Guid rowguid), PendingOpInfo> _rowOpInfoByRow = new();

        protected Guid? _activeEditRowguid;
        protected EntityLevel? _activeEditLevel;

        protected readonly Dictionary<Guid, bool> _isLightBg = new();

        protected object? _draftPlan { get; set; }
        protected object? _draftRub { get; set; }
        protected object? _draftFmt { get; set; }
        protected object? _draftTie { get; set; }
        protected object? _draftAct { get; set; }
        protected object? _draftAdt { get; set; }
        protected object? _draftRes { get; set; }
        protected object? _draftRdt { get; set; }
        protected object? _draftBro { get; set; }

        protected MultiLevelCrudBase(IPendingChangesGuard guard)
        {
            Guard = guard;
        }

        protected override void OnParametersSet()
        {
            SubscribeToGuard();
        }

        protected abstract void SubscribeToGuard();
        protected abstract string GetEntitySet(EntityLevel level);
        protected abstract void AddToLocalCollection(EntityLevel level, object entity);
        protected abstract void RemoveFromLocalCollection(EntityLevel level, object entity);
        protected abstract void RollbackPendingState(EntityLevel level, object entity, bool isNew);
        protected abstract void ReplaceInLocalCollection(EntityLevel level, object entity);
        protected abstract void CopyDraftToGridItem(EntityLevel level, object entity);
        protected abstract void FinalizeConfirmedState(EntityLevel level, object entity, string message);
        protected abstract void EndEdit(EntityLevel level);
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

        protected object DeepClone(object entity)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(entity, settings);
            return JsonConvert.DeserializeObject(json, entity.GetType(), settings)!;
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

        protected async Task ConfirmAdd(EntityLevel level, object? draft, bool isConfirm)
        {
            await UnifiedAddAction(level, draft, isConfirm);
            EndEdit(level);
        }

        protected async Task ConfirmEdit(EntityLevel level, object? draft, bool isConfirm)
        {
            Console.WriteLine("11EDIT PASS");
            //var draft = GetDraft(level);
            Console.WriteLine("22EDIT PASS");
            if (draft is null) return;

            Console.WriteLine("33EDIT PASS");
            await UnifiedEditAction(level, draft, isConfirm);
            EndEdit(level);
        }

        protected async Task ConfirmDelete(EntityLevel level, object entity, bool isConfirm)
        {
            await UnifiedDeleteAction(level, entity, isConfirm);
            EndEdit(level);
        }

        protected async Task CancelAdd(EntityLevel level, object entity)
        {
            await UnifiedCancelAction(level, entity, PendingOpType.Insert);
            EndEdit(level);
        }

        protected async Task CancelEdit(EntityLevel level, object entity)
        {
            await UnifiedCancelAction(level, entity, PendingOpType.Update);
            EndEdit(level);
        }

        protected async Task CancelDelete(EntityLevel level, object entity)
        {
            await UnifiedCancelAction(level, entity, PendingOpType.Delete);
            EndEdit(level);
        }
        private async Task UnifiedAddAction(EntityLevel level, object entity, bool isConfirm)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);
            if (!isConfirm)
            {
                // Cancel new row → remove from local collection
                RemoveFromLocalCollection(level, entity);
                RollbackPendingState(level, entity, true);
                return;
            }

            if (!Validate(level, entity))
                return;

            // Copy draft values into the grid item
            CopyDraftToGridItem(level, entity);

            var entitySet = GetEntitySet(level);

            // 🔥 Always insert for new rows
            var opId = await Guard.TrackInsert(entitySet, entity);

            SetPendingOpType(level, rowguid, PendingOpType.Insert);
            SetOpInfo(level, rowguid, new PendingOpInfo(opId, PendingOpType.Insert));

            // Ensure the new entity is in the local collection
            AddToLocalCollection(level, entity);

            FinalizeConfirmedState(level, entity, $"✅ {entity.GetType().Name} tracked (insert)");
        }

        private async Task UnifiedEditAction(EntityLevel level, object entity, bool isConfirm)
        {
            Console.WriteLine($"EDIT Rowguid here");
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);
            Console.WriteLine($"EDIT Rowguid {rowguid}");
            if (!isConfirm)
            {
                RollbackPendingState(level, entity, false);
                return;
            }

            if (!Validate(level, entity))
                return;

            CopyDraftToGridItem(level, entity);

            var entitySet = GetEntitySet(level);
            var key = MLEntityKeyHelper.GetKeyAsObject(entity);
            var opId = await Guard.TrackUpdate(entitySet, key, entity);

            SetPendingOpType(level, rowguid, PendingOpType.Update);
            SetOpInfo(level, rowguid, new PendingOpInfo(opId, PendingOpType.Update));

            ReplaceInLocalCollection(level, entity);
            FinalizeConfirmedState(level, entity, $"✅ {entity.GetType().Name} tracked");
        }

        private async Task UnifiedDeleteAction(EntityLevel level, object entity, bool isConfirm)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);

            if (!isConfirm)
            {
                SetRowState(level, rowguid, RowState.Default);
                return;
            }

            var entitySet = GetEntitySet(level);
            var key = MLEntityKeyHelper.GetKeyAsObject(entity);
            var opId = await Guard.TrackDelete(entitySet, key, rowguid);

            SetPendingOpType(level, rowguid, PendingOpType.Delete);
            SetOpInfo(level, rowguid, new PendingOpInfo(opId, PendingOpType.Delete));

            RemoveFromLocalCollection(level, entity);
            FinalizeConfirmedState(level, entity, $"✅ {entity.GetType().Name} deletion tracked");
        }

        private async Task UnifiedCancelAction(EntityLevel level, object entity, PendingOpType opType)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);
            var opInfo = GetOpInfo(level, rowguid);

            if (opInfo is null)
            {
                RollbackPendingState(level, entity, opType == PendingOpType.Insert);
                return;
            }

            await Guard.CancelTrackAsync(opInfo.OpId, opType);
            RollbackPendingState(level, entity, opType == PendingOpType.Insert);
        }

        //ROW MANAGEMENT
        protected void StartRowEdit(Guid rowguid, EntityLevel level)
        {
            _activeEditRowguid = rowguid;
            _activeEditLevel = level;
        }

        protected void EndRowEdit()
        {
            _activeEditRowguid = null;
            _activeEditLevel = null;
        }
        protected string GetRowCssFor(EntityLevel level, Guid rowguid)
        {
            var state = GetRowState(level, rowguid);
            var isLight = IsLightBackground(rowguid);
            var isActiveEdit = _activeEditRowguid == rowguid;
            var isOtherRowEditing = _activeEditRowguid != null && !isActiveEdit;

            return GetRowCss(state, isLight, rowguid, isActiveEdit, isOtherRowEditing);
        }

        protected string GetPlanRowClass(PlngenDto pln) => GetRowCssFor(EntityLevel.Plan, pln.Rowguid);
        protected string GetRubRowClass(RubvarDto rub) => GetRowCssFor(EntityLevel.Rub, rub.Rowguid);
        protected string GetFmtRowClass(RubfmtDto fmt) => GetRowCssFor(EntityLevel.Fmt, fmt.Rowguid);
        protected string GetTieRowClass(TierspDto tie) => GetRowCssFor(EntityLevel.Tie, tie.Rowguid);
        protected string GetActRowClass(ActsaieDto act) => GetRowCssFor(EntityLevel.Act, act.Rowguid);
        protected string GetAdtRowClass(ActdetDto adt) => GetRowCssFor(EntityLevel.Adt, adt.Rowguid);
        protected string GetResRowClass(ResdonDto res) => GetRowCssFor(EntityLevel.Res, res.Rowguid);
        protected string GetRdtRowClass(ResdetDto rdt) => GetRowCssFor(EntityLevel.Rdt, rdt.Rowguid);
        protected string GetBroRowClass(ResbroDto bro) => GetRowCssFor(EntityLevel.Bro, bro.Rowguid);

        protected RowState GetRowState(EntityLevel level, Guid rowguid)
            => _rowStates.TryGetValue((level, rowguid), out var value) ? value : RowState.Default;

        protected void SetRowState(EntityLevel level, Guid rowguid, RowState state)
            => _rowStates[(level, rowguid)] = state;

        protected void StartRowDelete(EntityLevel level, Guid rowguid)
        {
            SetRowState(level, rowguid, RowState.DeletePending);
            StartRowEdit(rowguid, level);
        }

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
    }
    //public record class PendingOpInfo(Guid OpId, PendingOpType OpType);
}
