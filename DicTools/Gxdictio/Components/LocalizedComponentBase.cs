using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

using GxDicto.Services;

namespace GxDicto.Components
{
    public class LocalizedComponentBase : ComponentBase
    {
        [Inject] protected ILocalizationService L { get; set; } = default!;

        protected string T(string key, string? defaultText = null)
            => L.T(key, defaultText);
    }
}
