using Radzen;
using Radzen.Blazor;

namespace HAN.Client.Components.UI;

public class PrimaryCard : RadzenCard
{
    protected override void OnInitialized()
    {
        Variant = Variant.Flat;
        base.OnInitialized();
    }
}