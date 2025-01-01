using HAN.Services.Dummy;
using HAN.Services.Extensions;
using HAN.Services.Interfaces;
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

builder.Services.AddCourseServices();

var app = builder.Build();

var scope = app.Services.CreateScope();
DataSeeder.SeedCourseData(scope.ServiceProvider);

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