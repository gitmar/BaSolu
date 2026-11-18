using GxPilo.Components.Uifrags;

using GxShared.GxDtos;
using GxShared.Helpers;
using GxShared.Interfaces;

namespace GxPilo.Components.Admina
{
    public partial class X2Gsafil : CompUICrudBase
    {
        public X2Gsafil(IPendingChangesGuard guard) : base(guard)
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
                case EntityLevel.Gf2:
                    var src2 = (GsgfixDto)entity;
                    var target2 = MyDaAfils.FirstOrDefault(x => x.Rowguid == src2.Rowguid);
                    if (target2 != null) CommitGfxDraft(target2, src2);
                    break;
            }
        }

        protected override void ClearAddRow(EntityLevel level, Guid rowguid)
        {
            switch (level)
            {
                case EntityLevel.Gfx:
                    draftGfx = null;
                    break;
                case EntityLevel.Gt2:
                    draftGf2 = null;
                    break;
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
                case EntityLevel.Gf2:
                    var idx2 = Gf2Items.FindIndex(x => x.Rowguid == rowguid);
                    if (idx2 >= 0)
                        Gf2Items[idx2] = DeepClone(_edGf2Originals[rowguid]);
                    draftGf2 = null;
                    break;
            }
        }
    }
}
