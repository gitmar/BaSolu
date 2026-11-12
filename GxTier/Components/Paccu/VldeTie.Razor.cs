using GxShared.GxDtos;
using GxShared.Helpers;
using GxShared.Interfaces;

using GxTie.Components.Uifrags;

namespace GxTie.Components.Paccu
{
    public partial class VldeTie : CompUICrudBase
    {
        public VldeTie(IPendingChangesGuard guard) : base(guard)
        {
        }

        protected override void SubscribeToGuard()
        {
        }

        protected override string GetEntitySetName(EntityLevel level)
        {
            return base.GetEntitySetName(level);
        }
        //protected override void ConfirmAdd(EntityLevel level, object entity)
        //{ }
        //protected override void ConfirmEdit(EntityLevel level, object entity)
        //{ }
        //protected override void ConfirmCancel(EntityLevel level, object entity)
        //{ }
        //protected override void ConfirmDelete(EntityLevel level, object entity)
        //{ }
        //protected override void RemoveFromLocalCollection(EntityLevel level, object entity)
        //{ }
        protected override void OnEntitySaved(EntityLevel level, object entity)
        {
            switch (level)
            {
                case EntityLevel.Tie:
                    {
                        var src = (TierspDto)entity;
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
                case EntityLevel.Wfl:
                    {
                        var src = (TiwaflDto)entity;
                        var target = MyDaWfls.FirstOrDefault(x => x.Rowguid == src.Rowguid);
                        if (target != null) CommitWflDraft(target, src);
                        break;
                    }
            }
        }

        //private void curPlanVue(int xvue)
        //{

        //}
        //private void curPlanFor(int xvue)
        //{

        //}
        //private void curPlanTier(int xtie)
        //{
        //    if (IMyDom != 0 && IMyVue != 0)
        //        InvokeAsync(StateHasChanged);
        //}
    }
}
