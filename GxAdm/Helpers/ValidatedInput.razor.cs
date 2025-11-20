using GxAdm.Helpers;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace GxAdm.Helpers;

public partial class ValidatedInput : ComponentBase
{
    [Parameter] public string RawValue { get; set; }
    [Parameter] public EventCallback<string> RawValueChanged { get; set; }

    [Parameter] public EventCallback<object> OnValidInput { get; set; }
    [Parameter] public EventCallback<bool> OnValidationStateChanged { get; set; }

    [Parameter] public int Atyp { get; set; } = 1;
    [Parameter] public string ErrorMessage { get; set; } = "Invalid input";

    protected bool IsValid { get; set; } = true;

    protected string CssClass => IsValid ? "form-control" : "form-control is-invalid";

    protected async Task Validate(FocusEventArgs args)
    {
        var (result, success) = RawValue.ConvertUserInputSafe(Atyp);
        IsValid = success;

        await OnValidationStateChanged.InvokeAsync(success);

        if (success)
        {
            await OnValidInput.InvokeAsync(result);
        }

        await RawValueChanged.InvokeAsync(RawValue);
    }
}