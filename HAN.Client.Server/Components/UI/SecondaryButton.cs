﻿using Radzen;
using Radzen.Blazor;

namespace HAN.Client.Server.Components.UI;

public class SecondaryButton : RadzenButton
{
    protected override void OnInitialized()
    {
        ButtonStyle = ButtonStyle.Light;
        Variant = Variant.Flat;
        Shade = Shade.Lighter;
        base.OnInitialized();
    }
}