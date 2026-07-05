using GxShared.GxDtos;
using GxShared.Helpers;
using GxShared.Interfaces;

using GxPilo.Components.Uifrags;

namespace GxPilo.Components.Admina.MRubs
{
    public partial class RubPosts : CompUICrudBase
    {
        public RubPosts(IPendingChangesGuard guard) : base(guard)
        {
        }

        protected override void SubscribeToGuard()
        {
        }

        protected override string GetEntitySet(EntityLevel level) => level switch
        {
            EntityLevel.Plan => "Plngens",
            EntityLevel.Hie => "Rubhies",
            EntityLevel.Pst => "Rubpsts",
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
                //case EntityLevel.Rub:
                //    {
                //        var src = (RubvarDto)entity;
                //        var target = MyDaRubrs.FirstOrDefault(x => x.Rowguid == src.Rowguid);
                //        if (target != null) CommitRubDraft(target, src);t
                //        break;
                //    }
                //case EntityLevel.Fmt:
                //    {
                //        var src = (RubfmtDto)entity;
                //        var target = MyDaFmts.FirstOrDefault(x => x.Rowguid == src.Rowguid);
                //        if (target != null) CommitRubDraft(target, src);
                //        break;
                //    }
                case EntityLevel.Hie:
                    {
                        var src = (RubhieDto)entity;
                        var target = MyDaHies.FirstOrDefault(x => x.Rowguid == src.Rowguid);
                        if (target != null) CommitHieDraft(target, src);
                        break;
                    }
                case EntityLevel.Pst:
                    {
                        var src = (RubpstDto)entity;
                        var target = MyDaPsts.FirstOrDefault(x => x.Rowguid == src.Rowguid);
                        if (target != null) CommitPstDraft(target, src);
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

                case EntityLevel.Hie:
                    draftRub = null;
                    RubRenderKey = Guid.Empty;
                    break;

                case EntityLevel.Pst:
                    draftPst = null;
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
                case EntityLevel.Hie:
                    var idxr = HieItems.FindIndex(x => x.Rowguid == rowguid);
                    if (idxr >= 0)
                        HieItems[idxr] = DeepClone(_edHieOriginals[rowguid]);
                    draftRub = null;
                    RubRenderKey = Guid.Empty;
                    break;
                case EntityLevel.Pst:
                    var idxf = PstItems.FindIndex(x => x.Rowguid == rowguid);
                    if (idxf >= 0)
                        PstItems[idxf] = DeepClone(_edPstOriginals[rowguid]);
                    draftPst = null;
                    FmtRenderKey = Guid.Empty;
                    break;
            }
        }
        private void curPlanDom(int domId)
        {
            //IMyDom = domId;
            if (IMyDom != 0)
            {
                int gtib = 7;
                if (IMyDom == 1) gtib = 6;
                LsGties = LsFixes.Where(ut => ut.Gvars == 36 && ut.Itb == gtib && ut.Elea != 0 && ut.Elea != 9).ToList();
            }
            else
            {
                LsGties = new();
            }
            // Example: check if all selections are valid
            if (IMyDom != 0 && IMyVue != 0)
            {
                // All three have values → single render
                InvokeAsync(StateHasChanged);
            }
        }
        private void curPlanAtr(int atrId)
        {
            //IMyAtr = atrId;

            if (IMyDom != 0 && IMyVue != 0)
            {
                InvokeAsync(StateHasChanged);
            }
        }
        private void curPlanVue(int vueId)
        {
            //IMyVue = vueId;

            if (IMyDom != 0 && IMyVue != 0)
            {
                InvokeAsync(StateHasChanged);
            }
        }
        private void UpdateAllDom(int domId, int vueId)
        {
            IMyDom = domId;
            //IMyAtr = atrId;
            IMyVue = vueId;

            InvokeAsync(StateHasChanged); // single refresh
        }
        private void ResetAllDom()
        {
            UpdateAllDom(domId: 0, vueId: 1);
        }
        private void curPlanFor(int xvue)
        {

        }
        //private void curPlanTier(int xtie)
        //{
        //    if (IMyDom != 0 && IMyAtr != 0 && IMyVue != 0)
        //        InvokeAsync(StateHasChanged);
        //}
    }
}