using HAN.Client.Server.Components.Base;
using HAN.Client.Server.Components;
using HAN.Services.Dummy;
using HAN.Services.Extensions;
using QuestPDF.Infrastructure;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Configure QuestPDF license
QuestPDF.Settings.License = LicenseType.Community;

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRadzenComponents();
builder.Services.AddCourseServices(builder.Configuration);
builder.Services.AddScoped<SystemFeedbackNotificationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseExceptionHandler("/Error");

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

var scope = app.Services.CreateScope();
DataSeeder.SeedCourseData(scope.ServiceProvider);

await app.RunAsync();