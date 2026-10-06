using GxShared.GxDtos;
using GxShared.Helpers;
using GxShared.Interfaces;

using GxTie.Components.Uifrags;

namespace GxTie.Components.Paccu
{
    public partial class ValdTie : CompUICrudBase
    {
        public ValdTie(IPendingChangesGuard guard) : base(guard)
        {
        }

        protected override void SubscribeToGuard()
        {
        }

        protected override string GetEntitySetName(EntityLevel level)
        {
            return base.GetEntitySetName(level);
        }
        protected override void OnEntitySaved(EntityLevel level, object entity)
        {
            switch (level)
            {
                case EntityLevel.Tie:
                    {
                        var src = (TiewelDto)entity;
                        var target = MyDaTies.FirstOrDefault(x => x.Rowguid == src.Rowguid);
                        if (target != null) CommitTieDraft(target, src);
                        break;
                    }
                case EntityLevel.Afl:
                    {
                        var src = (TieaflDto)entity;
                        var target = MyDaAfls.FirstOrDefault(x => x.Rowguid == src.Rowguid);
                        if (target != null) CommitAflDraft(target, src);
                        break;
                    }
            }
        }
    }
}
