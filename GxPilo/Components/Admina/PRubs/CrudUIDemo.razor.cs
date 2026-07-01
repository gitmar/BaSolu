using GxPilo.Components.Plans;

using GxShared.Helpers;
using GxShared.Helpers.CrudHelpers;
using GxShared.Interfaces;

namespace GxPilo.Components.Admina.PRubs
{
    public partial class CrudUIDemo : CompUICrudBase
    {
        public CrudUIDemo(IPendingChangesGuard guard) : base(guard)
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
            EntityLevel.Tie => "Tiersps",
            EntityLevel.Act => "Actsaies",
            EntityLevel.Adt => "Actdets",
            EntityLevel.Res => "Resdons",
            EntityLevel.Rdt => "Resdets",
            EntityLevel.Bro => "Resbros",
            _ => throw new ArgumentOutOfRangeException(nameof(level))
        };
        protected override ClearAddRow(EntityLevel level, Guid rowguid)
        {
            switch (level)
            {
                case EntityLevel.Plan:
                    PlanItems.RemoveAll(x => x.Rowguid == rowguid);
                    draftPln = null;
                    PlnRenderKey = Guid.Empty;
                    break;

                case EntityLevel.Rub:
                    RubItems.RemoveAll(x => x.Rowguid == rowguid);
                    draftRub = null;
                    RubRenderKey = Guid.Empty;
                    break;

                case EntityLevel.Fmt:
                    FmtItems.RemoveAll(x => x.Rowguid == rowguid);
                    draftFmt = null;
                    FmtRenderKey = Guid.Empty;
                    break;
            }

            await InvokeAsync(StateHasChanged);
        }
        protected async Task ClearEditRow(EntityLevel level, object entity)
        {
            var rowguid = MLEntityKeyHelper.GetRowguidAsGuid(entity);
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
    }
}