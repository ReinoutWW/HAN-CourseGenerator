using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HAN.Client;
using HAN.Client.Data;
using HAN.Data;
using HAN.Services.Extensions;
using Microsoft.EntityFrameworkCore;
using Radzen;
using NotificationService = HAN.Client.Components.Base.NotificationService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("InMem"));

builder.Services.AddRadzenComponents();
builder.Services.AddCourseServices();
builder.Services.AddScoped<NotificationService>();

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddOidcAuthentication(options =>
{
    // Configure your authentication provider options here.
    // For more information, see https://aka.ms/blazor-standalone-auth
    builder.Configuration.Bind("Local", options.ProviderOptions);
});

DataSeeder.SeedCourseData(builder.Build().Services);

await builder.Build().RunAsync();