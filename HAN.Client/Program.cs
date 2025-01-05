using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HAN.Client;
using HAN.Client.Auth;
using HAN.Client.Components.Base;
using HAN.Client.Services;
using HAN.Client.Services.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddRadzenComponents();
builder.Services.AddScoped<SystemFeedbackNotificationService>();

builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient<AuthorizationHttpHandler>();

builder.Services.AddHttpClient("AuthorizedClient", client =>
    {
        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    })
    .AddHttpMessageHandler<AuthorizationHttpHandler>();

// WebAPI Backend
var apiBaseUri = new Uri(builder.Configuration["CourseApi:BaseUrl"] ?? throw new ArgumentNullException(nameof(builder.Configuration), "CourseApi:BaseUrl is not configured."));
builder.Services.AddScoped(sp => apiBaseUri); // Web API Base URL

builder.Services.AddScoped<ICourseApiAdapter, CourseApiAdapter>(sp =>
{
    // Resolve the named HttpClient
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthorizedClient");
    var baseUri = sp.GetRequiredService<Uri>();
    return new CourseApiAdapter(httpClient, baseUri);
});

builder.Services.AddScoped<IProfileApiAdapter, ProfileApiAdapter>(sp =>
{
    // Resolve the named HttpClient
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthorizedClient");
    var baseUri = sp.GetRequiredService<Uri>();
    return new ProfileApiAdapter(httpClient, baseUri);
});
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<ITokenStorageService, TokenStorageService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var app = builder.Build();

var scope = app.Services.CreateScope();

await app.RunAsync();