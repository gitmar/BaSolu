using GxPilo.Components.Uifrags;

using GxShared.GxDtos;
using GxShared.Interfaces;

namespace GxPilo.Components.Admina
{
    public partial class ImmatPatternEditor : CompUICrudBase
    {
        public ImmatPatternEditor(IPendingChangesGuard guard) : base(guard)
        {
        }

        protected override void SubscribeToGuard()
        {
        }

        //protected override string GetEntitySetName(EntityLevel level)
        //{
        //    return base.GetEntitySetName(level);
        //}
        protected override void OnEntitySaved(object entity)
        {
            var src = (GsgfixDto)entity;
            var target = Segments.FirstOrDefault(x => x.Rowguid == src.Rowguid);
            //if (target != null) CommitPlnDraft(target, src);
            break;
        }
        protected override void ClearEditRow(Guid rowguid)
        {
            var idxp = Segments.FindIndex(x => x.Rowguid == rowguid);
            if (idxp >= 0)
                Segments[idxp] = DeepClone(_edFixOriginals[rowguid]);
            //draftPln = null;
            //PlnRenderKey = Guid.Empty;
            //Console.WriteLine($"Cancel row {rowguid}: draftPln={(draftPln == null ? "null" : "set")}, PlnRenderKey={PlnRenderKey}");
        }
    }
}