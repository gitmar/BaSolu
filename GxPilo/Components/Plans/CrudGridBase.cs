using GxShared.Helpers;
using GxShared.Interfaces;
using GxShared.Sess;
using GxShared.GxGuards;
using GxShared.Helpers.CrudHelpers;
// Infrastructure/Components/CrudGridBase.cs
using Microsoft.AspNetCore.Components;

namespace GxPilo.Components.Plans
{
    public abstract class CrudGridBase<TDto> : ComponentBase
        where TDto : class
    {
        protected readonly IPendingChangesGuard Guard;
        protected readonly string EntitySet;

        // UI state
        protected readonly Dictionary<Guid, RowState> _rowStates = new();
        protected readonly Dictionary<Guid, PendingOpType> _rowPendingOpTypeByRow = new();
        protected readonly Dictionary<Guid, PendingOpInfo> _rowOpInfoByRow = new();

        // Draft & editing
        protected TDto? Draft { get; set; }
        protected Guid? _currentlyEditingRowguid;

        public CrudGridBase(IPendingChangesGuard guard, string entitySet)
        {
            Guard = guard;
            EntitySet = entitySet;
        }

        protected override void OnParametersSet()
        {
            SubscribeToGuard();
        }

        protected abstract void SubscribeToGuard();

        // --- Row state helpers ---
        protected RowState GetRowState(Guid rowguid) => _rowStates.GetValueOrDefault(rowguid, RowState.Default);
        protected void SetRowState(Guid rowguid, RowState state) => _rowStates[rowguid] = state;

        protected PendingOpType GetPendingOpType(Guid rowguid) => _rowPendingOpTypeByRow.GetValueOrDefault(rowguid, PendingOpType.Update);
        protected void SetPendingOpType(Guid rowguid, PendingOpType op) => _rowPendingOpTypeByRow[rowguid] = op;

        protected void SetOpInfo(Guid rowguid, PendingOpInfo info) => _rowOpInfoByRow[rowguid] = info;
        protected PendingOpInfo? GetOpInfo(Guid rowguid) => _rowOpInfoByRow.GetValueOrDefault(rowguid);

        // --- CRUD actions ---
        protected async Task ConfirmAdd(bool isConfirm)
        {
            if (Draft is not TDto entity) return;
            await UnifiedAddAction(entity, isConfirm);
            EndEdit();
        }

        protected async Task ConfirmEdit(bool isConfirm)
        {
            if (Draft is not TDto entity) return;
            await UnifiedEditAction(entity, isConfirm);
            EndEdit();
        }

        protected async Task ConfirmDelete(TDto entity, bool isConfirm)
        {
            await UnifiedDeleteAction(entity, isConfirm);
            EndEdit();
        }

        protected async Task CancelAdd(TDto entity)
        {
            await UnifiedCancelAction(entity, PendingOpType.Insert);
            EndEdit();
        }

        protected async Task CancelEdit(TDto entity)
        {
            await UnifiedCancelAction(entity, PendingOpType.Update);
            EndEdit();
        }

        protected async Task CancelDelete(TDto entity)
        {
            await UnifiedCancelAction(entity, PendingOpType.Delete);
            EndEdit();
        }

        // --- Unified actions (generic) ---
        private async Task UnifiedAddAction(TDto entity, bool isConfirm)
        {
            var rowguid = EntityKeyHelper.GetRowguid(entity);
            var isNew = GetPendingOpType(rowguid) == PendingOpType.Insert;

            if (!isConfirm)
            {
                if (isNew)
                    RemoveFromLocalCollection(entity);
                RollbackPendingState(entity, isNew);
                return;
            }

            CopyDraftToGridItem(entity);

            Guid opId;
            if (isNew)
            {
                opId = await Guard.TrackInsert(EntitySet, entity);
                SetPendingOpType(rowguid, PendingOpType.Insert);
            }
            else
            {
                var key = EntityKeyHelper.GetKey(entity);
                opId = await Guard.TrackUpdate(EntitySet, key, entity);
                SetPendingOpType(rowguid, PendingOpType.Update);
            }

            SetOpInfo(rowguid, new PendingOpInfo(opId, isNew ? PendingOpType.Insert : PendingOpType.Update));

            if (isNew)
            {
                RemoveFromLocalCollection(entity);
                AddToLocalCollection(entity);
            }

            FinalizeConfirmedState(entity, $"✅ {typeof(TDto).Name} tracked");
        }

        private async Task UnifiedEditAction(TDto entity, bool isConfirm)
        {
            var rowguid = EntityKeyHelper.GetRowguid(entity);

            if (!isConfirm)
            {
                RollbackPendingState(entity, false);
                return;
            }

            if (!Validate(entity))
                return;

            CopyDraftToGridItem(entity);

            var key = EntityKeyHelper.GetKey(entity);
            var opId = await Guard.TrackUpdate(EntitySet, key, entity);
            SetPendingOpType(rowguid, PendingOpType.Update);
            SetOpInfo(rowguid, new PendingOpInfo(opId, PendingOpType.Update));

            ReplaceInLocalCollection(entity);

            FinalizeConfirmedState(entity, $"✅ {typeof(TDto).Name} tracked");
        }

        private async Task UnifiedDeleteAction(TDto entity, bool isConfirm)
        {
            var rowguid = EntityKeyHelper.GetRowguid(entity);

            if (!isConfirm)
            {
                SetRowState(rowguid, RowState.Default);
                return;
            }

            var key = EntityKeyHelper.GetKey(entity);
            var opId = await Guard.TrackDelete(EntitySet, key, rowguid);
            SetPendingOpType(rowguid, PendingOpType.Delete);
            SetOpInfo(rowguid, new PendingOpInfo(opId, PendingOpType.Delete));

            RemoveFromLocalCollection(entity);

            FinalizeConfirmedState(entity, $"✅ {typeof(TDto).Name} deletion tracked");
        }

        private async Task UnifiedCancelAction(TDto entity, PendingOpType opType)
        {
            var rowguid = EntityKeyHelper.GetRowguid(entity);
            var opInfo = GetOpInfo(rowguid);
            if (opInfo is null)
            {
                RollbackPendingState(entity, opType == PendingOpType.Insert);
                return;
            }

            await Guard.CancelTrackAsync(opInfo.OpId, opType);

            RollbackPendingState(entity, opType == PendingOpType.Insert);
        }

        // --- Abstract methods (per-level collections + validation) ---
        protected abstract bool Validate(TDto entity);
        protected abstract void AddToLocalCollection(TDto entity);
        protected abstract void RemoveFromLocalCollection(TDto entity);
        protected abstract void ReplaceInLocalCollection(TDto entity);

        // Deep clone via JSON
        protected TDto CloneDraft(TDto entity) => JsonCloner.Clone(entity)!;

        // Copy draft back to grid item (can be generic if you always use same pattern)
        protected abstract void CopyDraftToGridItem(TDto entity);

        protected abstract void RollbackPendingState(TDto entity, bool isNew);
        protected abstract void FinalizeConfirmedState(TDto entity, string message);
        protected abstract void EndEdit();
    }

    public record class PendingOpInfo(Guid OpId, PendingOpType OpType);
}
