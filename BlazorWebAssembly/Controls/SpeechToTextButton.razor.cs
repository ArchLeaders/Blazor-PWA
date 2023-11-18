using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;


namespace BlazorWebAssembly.Controls;

// Derived from https://github.com/radzenhq/radzen-blazor/
// Copyright (c) 2018-2023 Radzen Ltd

/// <summary>
/// Simple button for recording and transcripting speech to text
/// <br/><br/>
/// Derived from <a href="https://github.com/radzenhq/radzen-blazor/">Radzen Blazor</a><br/>
/// Copyright (c) 2018-2023 Radzen Ltd
/// </summary>
public partial class SpeechToTextButton : MudComponentBase
{
    /// <summary>
    /// The color of the component. It supports the theme colors.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Color Color { get; set; } = Color.Default;

    /// <summary>
    /// The Size of the component.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Size Size { get; set; } = Size.Medium;

    /// <summary>
    /// The variant to use.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Variant Variant { get; set; } = Variant.Filled;

    /// <summary>
    /// Child content of component.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Behavior)]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Icon placed before the text if set.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Behavior)]
    public string? Icon { get; set; } = Icons.Material.Filled.Mic;

    /// <summary>
    /// Gets or sets the icon displayed while recording.
    /// </summary>
    /// <value>The icon.</value>
    [Parameter]
    public string StopIcon { get; set; } = Icons.Material.Filled.Stop;

    private string? CurrentIcon => _recording ? StopIcon : Icon;

    /// <summary>
    /// Gets or sets the message displayed when user hovers the button and it is not recording.
    /// </summary>
    /// <value>The message.</value>
    [Parameter]
    public string Title { get; set; } = "Press to start speech recognition";

    /// <summary>
    /// Gets or sets the message displayed when user hovers the button and it is recording.
    /// </summary>
    /// <value>The message.</value>
    [Parameter]
    public string StopTitle { get; set; } = "Press to stop speech recognition";

    private string CurrentTitle => _recording ? StopTitle : Title;


    /// <summary>
    /// Callback which provides results from the speech recognition API.
    /// </summary>
    [Parameter]
    public EventCallback<string> Change { get; set; }

    /// <summary>
    /// Gets or sets the icon displayed while recording.
    /// </summary>
    /// <value>The icon.</value>
    [Parameter]
    public required string Language { get; set; }

    private bool _recording;

    private async Task StartSpeechToText()
    {
        _recording = !_recording;

        await JSRuntime.InvokeVoidAsync("BlazorWebAssembly.toggleDictation", Reference, Language);
    }

    /// <summary>
    /// Provides interface for javascript to stop speech to text recording on this component if another component starts recording.
    /// </summary>
    [JSInvokable]
    public void StopRecording()
    {
        _recording = false;

        StateHasChanged();
    }

    /// <summary>
    /// Provides interface for javascript to pass speech results back to this component.
    /// </summary>
    /// <param name="result"></param>
    [JSInvokable]
    public void OnResult(string result)
    {
        Change.InvokeAsync(result);
    }

    private DotNetObjectReference<SpeechToTextButton>? _reference;

    /// <summary>
    /// Gets the reference for the current component.
    /// </summary>
    /// <value>The reference.</value>
    protected DotNetObjectReference<SpeechToTextButton> Reference
        => _reference ??= DotNetObjectReference.Create(this);
}
