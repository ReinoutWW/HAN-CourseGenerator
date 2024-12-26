using Radzen;
using Radzen.Blazor;

namespace HAN.Client.Components.UI;

public class PrimaryButton : RadzenButton
{
    protected override void OnInitialized()
    {
        ButtonStyle = ButtonStyle.Primary;
        Variant = Variant.Flat;
        base.OnInitialized();
    }
}