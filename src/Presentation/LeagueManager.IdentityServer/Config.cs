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
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "https://desktop-3pdt884/LeagueManager.WebUI/signin-oidc" },
                    PostLogoutRedirectUris = { "https://desktop-3pdt884/LeagueManager.WebUI/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "sportapi",
                        "competitionapi",
                        "countryapi",
                        "teamapi",
                        "playerapi"
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

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new ApiScope[]
            {
                new ApiScope("sportapi", "LeagueManager Sport API"),
                new ApiScope("competitionapi", "LeagueManager Competition API"),
                new ApiScope("countryapi", "LeagueManager Country API"),
                new ApiScope("teamapi", "LeagueManager Team API"),
                new ApiScope("playerapi", "LeagueManager Player API")
            };
        }
    }
}