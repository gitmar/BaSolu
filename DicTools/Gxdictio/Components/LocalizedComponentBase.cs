using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using global::GxDicto.Services;
using Microsoft.AspNetCore.Components;

// Localization/Components/LocalizedComponentBase.cs
namespace GxDicto.Components
{
    public class LocalizedComponentBase : ComponentBase
    {
        [Inject] protected ILocalizationService L { get; set; } = default!;

        protected string T(string key, string? defaultText = null)
        => L.T(key, defaultText);
    }
}