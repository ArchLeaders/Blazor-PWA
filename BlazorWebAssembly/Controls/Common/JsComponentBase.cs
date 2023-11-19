using Microsoft.JSInterop;
using MudBlazor;

namespace BlazorWebAssembly.Controls.Common;

public class JsComponentBase : MudComponentBase
{
    private DotNetObjectReference<JsComponentBase>? _reference;

    /// <summary>
    /// Gets the reference for the current component.
    /// </summary>
    /// <value>The reference.</value>
    protected DotNetObjectReference<JsComponentBase> Reference
        => _reference ??= DotNetObjectReference.Create(this);

    ~JsComponentBase()
    {
        _reference?.Dispose();
    }
}
