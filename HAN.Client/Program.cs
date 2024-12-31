using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HAN.Client;
using HAN.Client.Authentication;
using HAN.Client.Components.Base;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddRadzenComponents();
builder.Services.AddScoped<SystemFeedbackNotificationService>();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

var app = builder.Build();

await app.RunAsync();