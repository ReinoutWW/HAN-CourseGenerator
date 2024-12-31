using Radzen;
using Radzen.Blazor;

namespace HAN.Client.Server.Components.UI;

public class PrimaryButton : RadzenButton
{
    protected override void OnInitialized()
    {
        ButtonStyle = ButtonStyle.Primary;
        Variant = Variant.Flat;
        Shade = Shade.Lighter;
        base.OnInitialized();
    }
}