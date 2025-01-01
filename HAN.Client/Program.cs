using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HAN.Client;
using HAN.Client.Auth;
using HAN.Client.Components.Base;
using HAN.Client.Services;
using HAN.Services.Dummy;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddRadzenComponents();
builder.Services.AddScoped<SystemFeedbackNotificationService>();

builder.Services.AddScoped<CourseService>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped(sp => new Uri("https://localhost:5005"));
builder.Services.AddScoped<ICourseApiAdapter, CourseApiAdapter>(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    var baseUri = sp.GetRequiredService<Uri>();
    return new CourseApiAdapter(httpClient, baseUri);
});

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();

var app = builder.Build();

var scope = app.Services.CreateScope();

await app.RunAsync();