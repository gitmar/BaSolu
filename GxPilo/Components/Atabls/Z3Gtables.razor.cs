using GxShared.GxDtos;
using GxShared.Helpers;
using GxShared.Interfaces;

using GxPilo.Components.Uifrags;

namespace GxPilo.Components.Atabls
{
    public partial class Z3Gtables : CompUICrudBase
    {
        public Z3Gtables(IPendingChangesGuard guard) : base(guard)
        {
        }

        protected override void SubscribeToGuard()
        {
        }

        protected override string GetEntitySet(EntityLevel level) => level switch
        {
            EntityLevel.Gtb => "Gstabls",
            //EntityLevel.Rub => "Rubvars",
            //EntityLevel.Fmt => "Rubfmts",
            _ => throw new ArgumentOutOfRangeException(nameof(level))
        };
        protected override void OnEntitySaved(EntityLevel level, object entity)
        {
            switch (level)
            {
                case EntityLevel.Gtb:
                    {
                        var src = (GstablDto)entity;
                        var target = MyDaTabls.FirstOrDefault(x => x.Rowguid == src.Rowguid);
                        if (target != null) CommitGtbDraft(target, src);
                        break;
                    }
                case EntityLevel.Gt2:
                    {
                        var src = (GstablDto)entity;
                        var target = MyDaTabls.FirstOrDefault(x => x.Rowguid == src.Rowguid);
                        if (target != null) CommitGtbDraft(target, src);
                        break;
                    }
                    //case EntityLevel.Fmt:
                    //    {
                    //        var src = (RubfmtDto)entity;
                    //        var target = MyDaFmts.FirstOrDefault(x => x.Rowguid == src.Rowguid);
                    //        if (target != null) CommitFmtDraft(target, src);
                    //        break;
                    //    }
            }
        }
        protected override void ClearAddRow(EntityLevel level, Guid rowguid)
        {
            switch (level)
            {
                case EntityLevel.Plan:
                    draftFix = null;
                    FixRenderKey = Guid.Empty;
                    break;

                case EntityLevel.Rub:
                    draftChl = null;
                    ChldRenderKey = Guid.Empty;
                    break;

                //case EntityLevel.Fmt:
                //    draftFmt = null;
                //    FmtRenderKey = Guid.Empty;
                //    break;
            }

            //await InvokeAsync(StateHasChanged);
        }
        protected override void ClearEditRow(EntityLevel level, Guid rowguid)
        {
            switch (level)
            {
                case EntityLevel.Plan:
                    var idxp = GtbItems.FindIndex(x => x.Rowguid == rowguid);
                    if (idxp >= 0)
                        GtbItems[idxp] = DeepClone(_edFixOriginals[rowguid]);
                    draftFix = null;
                    FixRenderKey = Guid.Empty;
                    Console.WriteLine($"Cancel row {rowguid}: draftFix={(draftFix == null ? "null" : "set")}, FixRenderKey={FixRenderKey}");

                    break;
                case EntityLevel.Rub:
                    var idxr = GchItems.FindIndex(x => x.Rowguid == rowguid);
                    if (idxr >= 0)
                        GchItems[idxr] = DeepClone(_edFixOriginals[rowguid]);
                    draftChl = null;
                    ChldRenderKey = Guid.Empty;
                    break;
                //case EntityLevel.Fmt:
                //    var idxf = FmtItems.FindIndex(x => x.Rowguid == rowguid);
                //    if (idxf >= 0)
                //        FmtItems[idxf] = DeepClone(_edFmtOriginals[rowguid]);
                //    draftFmt = null;
                //    FmtRenderKey = Guid.Empty;
                //    break;
            }
        }
    }
}