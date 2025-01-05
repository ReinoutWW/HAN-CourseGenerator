using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;

namespace HAN.IdentityServer;
public static class Config
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("courseapi", "HAN Course API")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "courseapi" }
            }
        };

    public static List<TestUser> TestUsers =>
        new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "alice",
                Password = "password"
            },
            new TestUser
            {
                SubjectId = "2",
                Username = "bob",
                Password = "password"
            },
            new TestUser
            {
                SubjectId = "2",
                Username = "RG",
                Password = "password420"
            }
        };
    
    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource("courseapi", "HAN Course API")
            {
                Scopes = { "courseapi" }
            }
        };
}

