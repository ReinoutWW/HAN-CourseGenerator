using HAN.Client.API;
using HAN.Client.API.RabbitMQ;
using HAN.Services.Dummy;
using HAN.Services.Extensions;
using HAN.Services.Interfaces;
using HAN.Services.Messages;
using HAN.Utilities.Messaging.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

const string identityServerAuthority = "https://localhost:7054";

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder
                .WithOrigins("https://localhost:7149") // Allow your frontend origin
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = identityServerAuthority; 
        options.Audience = "courseapi";
        options.RequireHttpsMetadata = true; 

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = identityServerAuthority,
            ValidateAudience = true,
            ValidAudience = "courseapi",
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCourseServices(builder.Configuration);

builder.Services.AddSingleton<IServiceMessageHandler<CourseMessage>, CourseMessageHandler>(sp =>
{
    var courseService = sp.CreateScope().ServiceProvider.GetRequiredService<ICourseService>();
    return new CourseMessageHandler(courseService);
});

builder.Services.AddSingleton<IMessageBackgroundListenerService, RabbitMqListenerService>();
builder.Services.AddHostedService<MessageHandlerBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

var scope = app.Services.CreateScope();
DataSeeder.SeedCourseData(scope.ServiceProvider);

app.MapCourseEndpoints();

app.MapGet("/", () => "Minimal API is running.").AllowAnonymous();

app.Run();