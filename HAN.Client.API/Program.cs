using System.Net.Http.Headers;
using System.Security.Claims;
using HAN.Client.API;
using HAN.Services.Extensions;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCourseServices();

builder.Services.AddAuthentication()
    .AddCookie("cookie", o =>
    {
        o.LoginPath = "/login";
        
        var del = o.Events.OnRedirectToAccessDenied;
        o.Events.OnRedirectToAccessDenied = ctx =>
        {
            if(ctx.Request.Path.StartsWithSegments("/google"))
            {
                return ctx.HttpContext.ChallengeAsync("google");
            }

            return del(ctx);
        };
    })
    .AddOAuth("google", o =>
    {
        o.SignInScheme = "cookie";
        o.ClientId = Secrets.GoogleClientId;
        o.ClientSecret = Secrets.GoogleClientSecret;
        o.SaveTokens = false;
        
        o.Scope.Clear();
        o.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
        
        o.AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        o.TokenEndpoint = "https://oauth2.googleapis.com/token";
        o.CallbackPath = "/oauth/han-coursegenerator";

        o.Events.OnCreatingTicket = async ctx =>
        {
            var db = ctx.HttpContext.RequestServices.GetRequiredService<DataBase>();
            var authenticationHandlerProvider = ctx.HttpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            var handler = await authenticationHandlerProvider.GetHandlerAsync(ctx.HttpContext, "cookie");

            var authResult = await handler.AuthenticateAsync();

            if (authResult.Succeeded == false)
            {
                ctx.Fail("Failed authentication");
                return;
            }

            var cp = authResult.Principal;
            var userId = cp.FindFirstValue("user_id");
            db[userId] = ctx.AccessToken;

            ctx.Principal = cp.Clone();
            var identity = ctx.Principal.Identities.First(x => x.AuthenticationType == "cookie");
            identity.AddClaim(new Claim("coursebuilder", "y"));
        };
    });

builder.Services.AddHttpClient();

builder.Services.AddSingleton<DataBase>()
    .AddTransient<IClaimsTransformation, GoogleTokenClaimsTransformation>();

builder.Services.AddAuthorization(b =>
{
    b.AddPolicy("google-enabled", pb =>
    {
        pb.AddAuthenticationSchemes("cookie")
            .RequireClaim("coursebuilder", "y")
            .RequireAuthenticatedUser();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/login", () => Results.SignIn(
    new ClaimsPrincipal(
        new ClaimsIdentity(
            [new Claim("user_id", Guid.NewGuid().ToString())],
              "cookie"
        )
    ),
    authenticationScheme: "cookie"
));

app.MapGet("/google/info", async (IHttpClientFactory clientFactory, DataBase db, HttpContext ctx) =>
{
    var accessToken = ctx.User.FindFirstValue("coursebuilder-access-token");
    var client = clientFactory.CreateClient();

    using var req = new HttpRequestMessage(HttpMethod.Get, "https://www.googleapis.com/oauth2/v1/userinfo");
    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    
    var respone = await client.SendAsync(req);
    
    return await respone.Content.ReadAsStringAsync();
}).RequireAuthorization("google-enabled");

app.Run();

public class DataBase : Dictionary<string, string>
{
    
}

public class GoogleTokenClaimsTransformation : IClaimsTransformation
{
    private readonly DataBase _db;

    public GoogleTokenClaimsTransformation(DataBase dataBase)
    {
        _db = dataBase;
    }

    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var userId = principal.FindFirstValue("user_id");
        if(!_db.ContainsKey(userId))
        {
            return Task.FromResult(principal);
        }

        var accessToken = _db[userId];
        
        var cp = principal.Clone();
        var identity = cp.Identities.First(x => x.AuthenticationType == "cookie");
        identity.AddClaim(new Claim("coursebuilder-access-token", accessToken));
        
        return Task.FromResult(cp);
    }
}