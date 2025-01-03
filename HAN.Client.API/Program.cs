using System.Security.Claims;
using Auth0.AspNetCore.Authentication;
using HAN.Services.Dummy;
using HAN.Services.Extensions;
using HAN.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins",
        policyBuilder =>
        {
            // Replace with the specific origin(s) you want to allow
            policyBuilder
                .WithOrigins("https://localhost:7149")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});


builder.Services
    .AddAuth0WebAppAuthentication(options =>
    {
        options.Domain = builder.Configuration["Auth0:Domain"];
        options.ClientId = builder.Configuration["Auth0:ClientId"];
        // options.CallbackPath = "/login";
    });

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
});

builder.Services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, cookieOptions =>
{
    cookieOptions.LoginPath = "/login"; 
});

builder.Services.AddAuthorization();
builder.Services.AddHttpClient();

builder.Services.AddCourseServices();

var app = builder.Build();

app.UseCors("MyAllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();

var scope = app.Services.CreateScope();
DataSeeder.SeedCourseData(scope.ServiceProvider);

app.MapGet("/callback", () => Results.Ok("Hello, world!"));

app.MapGet("/login", IResult (HttpContext context, string returnUrl = "") =>
{
    var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
        .WithRedirectUri("https://localhost:7149/login")
        .Build();

    return Results.Challenge(
        authenticationProperties, 
        new[] { Auth0Constants.AuthenticationScheme }
    );
});

app.MapGet("/profile", [Authorize] IResult (HttpContext context, string returnUrl = "") =>
    {
        var user = context.User;
        var name = user.Identity?.Name;
        var emailAddress = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var profileImage = user.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;
        var nickname = user.Claims.FirstOrDefault(c => c.Type == "nickname")?.Value;

        // Return a JSON object
        return Results.Json(new Profile()
        {
            Name = name,
            EmailAddress = emailAddress,
            ProfileImage = profileImage,
            Nickname = nickname
        });
    })
    .WithName("Profile");

app.MapGet("/logout", [Authorize] async Task<IResult> (HttpContext context) =>
{
    // Sign out of local cookie first (no redirect)
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    
    // Then sign out of Auth0 (which includes a redirect)
    var logoutProperties = new LogoutAuthenticationPropertiesBuilder()
        .WithRedirectUri("https://rww.com:7149/login") 
        .Build();
    
    // This call will typically trigger an immediate 302 Redirect
    await context.SignOutAsync(Auth0Constants.AuthenticationScheme, logoutProperties);
    
    return Results.Ok(new { message = "You have been logged out." });
    // Don't do another redirect here.
    // Once the sign-out commits a 302, the request is done.
});


app.MapGet("/course", (ICourseService courseService) =>
{
    var courses = courseService.GetAllCourses();
    return Results.Ok(courses);
});

app.MapGet("/api/secure-data", [Authorize] () =>
{
    return Results.Ok(new { message = "This is secure data accessible to authenticated users." });
});

app.MapGet("/api/admin-data", [Authorize(Policy = "AdminPolicy")] () =>
{
    return Results.Ok(new { message = "This is admin-only data." });
});

app.UseCors(policy =>
    policy.WithOrigins("https://localhost:7149") // Blazor app URL
        .AllowAnyHeader()
        .AllowAnyMethod());

app.Run();

public class Profile
{
    public string Name { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string ProfileImage { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
}