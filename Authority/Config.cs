using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace Authority
{
    public static class Config
    {
        /*
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("QuotesApi", "Test quotes API"),
                new ApiResource("ThesesApi", "Theses API"),
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName),
            };
        }
        */

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "quotes.mvc",
                    ClientName = "Quotes MVC Client",
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "QuotesApi",
                        "email",
                        "phone",
                        "roles",
                        IdentityServerConstants.LocalApi.ScopeName
                    },
                    AllowOfflineAccess = true
                },
                new Client
                {
                    ClientId = "quotes.js",
                    ClientName = "Quotes Javascript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = false,
                    AllowAccessTokensViaBrowser = true,

                    // where to redirect to after login
                    RedirectUris = { "https://localhost:44310/oidc-callback", "https://localhost:44310/oidc-silent-renew" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:44310/oidc-signout-callback" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "QuotesApi",
                        "email"
                    },
                    AllowOfflineAccess = true
                },
                new Client
                {
                    ClientId = "theses.client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("theses-secret".Sha256())
                    },

                    AllowedScopes = { "ThesesApi" }
                },
                new Client
                {
                    ClientId = "theses.js",
                    ClientName = "Theses Javascript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = false,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { "http://localhost:3000/oidc-callback", "http://localhost:3000/oidc-silent-renew" },

                    PostLogoutRedirectUris = { "http://localhost:3000/oidc-signout-callback" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "ThesesApi",
                        "email"
                    },
                    AllowOfflineAccess = true
                },
                new Client
                {
                    ClientId = "rentals.js",
                    ClientName = "Rentals Javascript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = false,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { "http://localhost:3000/oidc-callback", "http://localhost:3000/oidc-silent-renew" },

                    PostLogoutRedirectUris = { "http://localhost:3000/oidc-signout-callback" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "RentalsApi",
                        "email"
                    },
                    AllowOfflineAccess = true
                },
                new Client
                {
                    ClientId = "rentals.mvc",
                    ClientName = "Rentals MVC Client",
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "RentalsApi",
                        "email",
                        "phone",
                        "roles",
                        IdentityServerConstants.LocalApi.ScopeName
                    },
                    AllowOfflineAccess = true
                },
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResource("roles", new List<string>{"role"})
            };
        }
    }
}
