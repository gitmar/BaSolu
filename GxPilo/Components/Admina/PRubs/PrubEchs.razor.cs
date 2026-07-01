using GxShared.GxDtos;
using GxShared.Helpers;
using GxShared.Interfaces;

using GxPilo.Components.Plans;

namespace GxPilo.Components.Admina.PRubs
{
    public partial class PrubEchs : CompUICrudBase
    {
        public PrubEchs(IPendingChangesGuard guard) : base(guard)
        {
        }

        protected override void SubscribeToGuard()
        {
        }

        protected override string GetEntitySet(EntityLevel level) => level switch
        {
            EntityLevel.Plan => "Plngens",
            EntityLevel.Rub => "Rubvars",
            EntityLevel.Fmt => "Rubfmts",
            _ => throw new ArgumentOutOfRangeException(nameof(level))
        };
        protected override void OnEntitySaved(EntityLevel level, object entity)
        {
            switch (level)
            {
                case EntityLevel.Plan:
                    {
                        var src = (PlngenDto)entity;
                        var target = MyDaPlans.FirstOrDefault(x => x.Rowguid == src.Rowguid);
                        if (target != null) CommitPlnDraft(target, src);
                        break;
                    }
                case EntityLevel.Rub:
                    {
                        var src = (RubvarDto)entity;
                        var target = MyDaRubs.FirstOrDefault(x => x.Rowguid == src.Rowguid);
                        if (target != null) CommitRubDraft(target, src);
                        break;
                    }
                case EntityLevel.Fmt:
                    {
                        var src = (RubfmtDto)entity;
                        var target = MyDaFmts.FirstOrDefault(x => x.Rowguid == src.Rowguid);
                        if (target != null) CommitFmtDraft(target, src);
                        break;
                    }
            }
        }
        protected override void ClearAddRow(EntityLevel level, Guid rowguid)
        {
            switch (level)
            {
                case EntityLevel.Plan:
                    draftPln = null;
                    PlnRenderKey = Guid.Empty;
                    break;

                case EntityLevel.Rub:
                    draftRub = null;
                    RubRenderKey = Guid.Empty;
                    break;

                case EntityLevel.Fmt:
                    draftFmt = null;
                    FmtRenderKey = Guid.Empty;
                    break;
            }

            //await InvokeAsync(StateHasChanged);
        }
        protected override void ClearEditRow(EntityLevel level, Guid rowguid)
        {
            switch (level)
            {
                case EntityLevel.Plan:
                    var idxp = PlanItems.FindIndex(x => x.Rowguid == rowguid);
                    if (idxp >= 0)
                        PlanItems[idxp] = DeepClone(_edPlnOriginals[rowguid]);
                    draftPln = null;
                    PlnRenderKey = Guid.Empty;
                    Console.WriteLine($"Cancel row {rowguid}: draftPln={(draftPln == null ? "null" : "set")}, PlnRenderKey={PlnRenderKey}");

                    break;
                case EntityLevel.Rub:
                    var idxr = RubItems.FindIndex(x => x.Rowguid == rowguid);
                    if (idxr >= 0)
                        RubItems[idxr] = DeepClone(_edRubOriginals[rowguid]);
                    draftRub = null;
                    RubRenderKey = Guid.Empty;
                    break;
                case EntityLevel.Fmt:
                    var idxf = FmtItems.FindIndex(x => x.Rowguid == rowguid);
                    if (idxf >= 0)
                        FmtItems[idxf] = DeepClone(_edFmtOriginals[rowguid]);
                    draftFmt = null;
                    FmtRenderKey = Guid.Empty;
                    break;
            }
        }
        private void curPlanTier(int xtie)
        {
            if (IMyDom != 0 && IMyAtr != 0 && IMyVue != 0)
                InvokeAsync(StateHasChanged);
        }
    }
}

