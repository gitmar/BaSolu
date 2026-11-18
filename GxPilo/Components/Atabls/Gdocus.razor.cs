using GxPilo.Components.Uifrags;

using GxShared.GxDtos;
using GxShared.Helpers;
using GxShared.Interfaces;

namespace GxPilo.Components.Atabls
{
    public partial class Gdocus : CompUICrudBase
    {
        public Gdocus(IPendingChangesGuard guard) : base(guard)
        {
        }

        protected override void SubscribeToGuard()
        {
        }
        protected override void OnEntitySaved(EntityLevel level, object entity)
        {
            switch (level)
            {
                case EntityLevel.Gfx:
                    var src = (GsgfixDto)entity;
                    var target = MyDaDocus.FirstOrDefault(x => x.Rowguid == src.Rowguid);
                    if (target != null) CommitGfxDraft(target, src);
                    break;
                    //case EntityLevel.Gf2:
                    //    {
                    //        var src = (GsgfixDto)entity;
                    //        var target = MyDaGt2s.FirstOrDefault(x => x.Rowguid == src.Rowguid);
                    //            if (target != null) CommitGt2Draft(target, src);
                    //        break;
                    //    }
            }
        }

        protected override void ClearAddRow(EntityLevel level, Guid rowguid)
        {
            switch (level)
            {
                case EntityLevel.Gfx:
                    draftGfx = null;
                    break;

                    //case EntityLevel.Gt2:
                    //    draftGt2 = null;
                    //    break;
            }
        }
        protected override void ClearEditRow(EntityLevel level, Guid rowguid)
        {
            switch (level)
            {
                case EntityLevel.Gfx:
                    var idxp = GfxItems.FindIndex(x => x.Rowguid == rowguid);
                    if (idxp >= 0)
                        GfxItems[idxp] = DeepClone(_edGfxOriginals[rowguid]);
                    draftGfx = null;
                    break;
                    //case EntityLevel.Gt2:
                    //    var idxr = Gt2Items.FindIndex(x => x.Rowguid == rowguid);
                    //    if (idxr >= 0)
                    //        Gt2Items[idxr] = DeepClone(_edGt2Originals[rowguid]);
                    //    draftGt2 = null;
                    //    break;
            }
        }
    }
}