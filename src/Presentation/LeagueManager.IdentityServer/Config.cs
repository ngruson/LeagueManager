using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace LeagueManager.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<Client> GetClients()
        {
            return new Client[]
            {
                new Client
                {
                    ClientId = "LeagueManager",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    ClientName = "LeagueManager Web Application",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = { "https://desktop-3pdt884/LeagueManager.WebUI/signin-oidc" },
                    PostLogoutRedirectUris = { "https://desktop-3pdt884/LeagueManager.WebUI/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "competitionapi",
                        "countryapi",
                        "teamapi"
                    },
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new ApiResource[]
            {
                new ApiResource("competitionapi", "LeagueManager Competition API"),
                new ApiResource("countryapi", "LeagueManager Country API"),
                new ApiResource("teamapi", "LeagueManager Team API")
            };
        }
    }
}