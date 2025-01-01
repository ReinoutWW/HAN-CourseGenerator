using HAN.Client.API;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

// Add authentication services
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "Bearer";
        options.DefaultChallengeScheme = "Bearer";
    })
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://accounts.google.com";
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = "1032361026808-0kumcpddiv8nt5f8b1q3sn4j4imb8ufu.apps.googleusercontent.com",
            ValidateIssuer = true,
            ValidIssuer = "https://accounts.google.com",
            ValidateLifetime = true
        };
    });

// Add authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Admin");
    });
});

var app = builder.Build();

// Use authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/auth-callback", (string code, string state) =>
{
    var token = new Guid().ToString("n");
    
    return Task.FromResult(Results.Redirect($"https://localhost:7149/auth?success=true&token={token}"));
});


// Secured endpoint: Accessible only to authenticated users
app.MapGet("/api/secure-data", [Authorize] () =>
{
    return Results.Ok(new { message = "This is secure data accessible to authenticated users." });
});

// Admin-only endpoint: Accessible only to users with the Admin role
app.MapGet("/api/admin-data", [Authorize(Policy = "AdminPolicy")] () =>
{
    return Results.Ok(new { message = "This is admin-only data." });
});

app.UseCors(policy =>
    policy.WithOrigins("https://localhost:7149") // Blazor app URL
        .AllowAnyHeader()
        .AllowAnyMethod());

app.Run();