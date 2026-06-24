using GxPilo.Components.Plans;

using GxShared.GxDtos;
using GxShared.Helpers;
using GxShared.Helpers.CrudHelpers;
using GxShared.Interfaces;
using GxShared.Sess;

using Microsoft.AspNetCore.Components;

using Newtonsoft.Json;

namespace GxPilo.Components.Admina
{
    public partial class OrgaCrudPage : MultiLevelCrudBase
    {
        [Inject] private IPendingChangesGuard Guard { get; set; } = default!;

        public List<PlngenDto> MyDaPlans { get; set; } = new();
        public List<RubvarDto> MyDaRubs { get; set; } = new();
        public List<RubfmtDto> MyDaFmts { get; set; } = new();

        public PlngenDto? selectedPlan { get; set; }

        public OrgaCrudPage(IPendingChangesGuard guard)
            : base(guard)
        {
        }

        protected override void SubscribeToGuard()
        {
            Guard.OnChanges += () =>
            {
                NbChanges = Guard.GetPendingChangesCount();
                StateHasChanged();
            };
        }

        protected override string GetEntitySet(EntityLevel level) =>
            level switch
            {
                EntityLevel.Plan => "Plngens",
                EntityLevel.Rub => "Rubvars",
                EntityLevel.Fmt => "Rubfmts",
                _ => throw new ArgumentOutOfRangeException(nameof(level))
            };

        // --- Validation ---
        protected override bool Validate(EntityLevel level, object entity)
        {
            return level switch
            {
                EntityLevel.Plan => entity is PlngenDto p && !string.IsNullOrWhiteSpace(p.Liba),
                EntityLevel.Rub => entity is RubvarDto r && !string.IsNullOrWhiteSpace(r.Liba),
                EntityLevel.Fmt => entity is RubfmtDto f && !string.IsNullOrWhiteSpace(f.Liba),
                _ => false
            };
        }

        // --- Collections ---
        protected override void AddToLocalCollection(EntityLevel level, object entity)
        {
            switch (level)
            {
                case EntityLevel.Plan:
                    MyDaPlans.Insert(0, (PlngenDto)entity);
                    break;
                case EntityLevel.Rub:
                    MyDaRubs.Insert(0, (RubvarDto)entity);
                    break;
                case EntityLevel.Fmt:
                    MyDaFmts.Insert(0, (RubfmtDto)entity);
                    break;
            }
        }

        protected override void RemoveFromLocalCollection(EntityLevel level, object entity)
        {
            switch (level)
            {
                case EntityLevel.Plan:
                    MyDaPlans.Remove((PlngenDto)entity);
                    break;
                case EntityLevel.Rub:
                    MyDaRubs.Remove((RubvarDto)entity);
                    break;
                case EntityLevel.Fmt:
                    MyDaFmts.Remove((RubfmtDto)entity);
                    break;
            }
        }

        protected override void ReplaceInLocalCollection(EntityLevel level, object entity)
        {
            switch (level)
            {
                case EntityLevel.Plan:
                    var pl = (PlngenDto)entity;
                    var idxP = MyDaPlans.FindIndex(p => p.Rowguid == pl.Rowguid);
                    if (idxP >= 0) MyDaPlans[idxP] = pl;
                    break;
                case EntityLevel.Rub:
                    var ru = (RubvarDto)entity;
                    var idxR = MyDaRubs.FindIndex(r => r.Rowguid == ru.Rowguid);
                    if (idxR >= 0) MyDaRubs[idxR] = ru;
                    break;
                case EntityLevel.Fmt:
                    var fm = (RubfmtDto)entity;
                    var idxF = MyDaFmts.FindIndex(f => f.Rowguid == fm.Rowguid);
                    if (idxF >= 0) MyDaFmts[idxF] = fm;
                    break;
            }
        }

        // --- Copy draft to grid item ---
        protected override void CopyDraftToGridItem(EntityLevel level, object entity)
        {
            var draft = GetDraft(level);
            if (draft is null) return;

            switch (level)
            {
                case EntityLevel.Plan:
                    CopyPlanDraft((PlngenDto)entity, (PlngenDto)draft);
                    break;
                case EntityLevel.Rub:
                    CopyRubDraft((RubvarDto)entity, (RubvarDto)draft);
                    break;
                case EntityLevel.Fmt:
                    CopyFmtDraft((RubfmtDto)entity, (RubfmtDto)draft);
                    break;
            }
        }

        private void CopyPlanDraft(PlngenDto entity, PlngenDto draft)
        {
            entity.Liba = draft.Liba;
            entity.Abg = draft.Abg;
            entity.Eta = draft.Eta;
            // ...
        }

        private void CopyRubDraft(RubvarDto entity, RubvarDto draft)
        {
            entity.Liba = draft.Liba;
            entity.Abg = draft.Abg;
            entity.Eta = draft.Eta;
            entity.Ecmount = draft.Ecmount;
            // ...
        }

        private void CopyFmtDraft(RubfmtDto entity, RubfmtDto draft)
        {
            entity.Liba = draft.Liba;
            entity.Abg = draft.Abg;
            entity.Eta = draft.Eta;
            // ...
        }

        // --- Rollback & finalize ---
        protected override void RollbackPendingState(EntityLevel level, object entity, bool isNew)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);
            if (isNew)
                RemoveFromLocalCollection(level, entity);

            SetRowState(level, rowguid, RowState.Default);
            _rowPendingOpTypeByRow.Remove((level, rowguid));
            _rowOpInfoByRow.Remove((level, rowguid));
            StateHasChanged();
        }

        protected override void FinalizeConfirmedState(EntityLevel level, object entity, string message)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);
            SetRowState(level, rowguid, RowState.Default);
            Imessage = message;
            StateHasChanged();
        }

        protected override void EndEdit(EntityLevel level)
        {
            SetDraft(level, null);
            StateHasChanged();
        }

        // --- UI helpers per level ---
        protected async Task StartEdit(EntityLevel level, object entity)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);
            SetRowState(level, rowguid, RowState.EditPending);
            SetPendingOpType(level, rowguid, PendingOpType.Update);

            var cloned = JsonCloner.Clone(entity);
            SetDraft(level, cloned);

            StateHasChanged();
        }

        protected async Task StartDelete(EntityLevel level, object entity)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);
            SetRowState(level, rowguid, RowState.DeletePending);
            StateHasChanged();
        }

        // --- Save all ---
        public int NbChanges { get; set; }
        public string? Imessage { get; set; }

        private async Task SaveAll()
        {
            if (!Guard.HasPendingChanges())
                return;

            await Guard.FlushAsync();
            // Optionally reload data
        }
    }
}
