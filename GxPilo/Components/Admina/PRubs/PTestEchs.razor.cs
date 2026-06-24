using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.Security.Claims;
using BlazorBootstrap;
using Blazored.LocalStorage;
using Newtonsoft.Json;

using GxPilo.Components.Plans;
using GxPilo.Services;

using GxShared.GxDtos;
using GxShared.Helpers;
using GxShared.Interfaces;
using GxShared.Sess;

using Simple.OData.Client;

namespace GxPilo.Components.Admina.PRubs
{
    public partial class PTestEchs : CompUICrudBase
    {
        public PTestEchs(IPendingChangesGuard guard) : base(guard)
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

        private void curPlanTier(int xtie)
        {
            if (IMyDom != 0 && IMyAtr != 0 && IMyVue != 0)
                InvokeAsync(StateHasChanged);
        }
    }
}
