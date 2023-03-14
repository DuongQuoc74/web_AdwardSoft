using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.API.Authentication.Configurations.IdentityServer
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("API.Core", "AdwardSoft API Core"),
                new ApiResource("grpc.demo", "AdwardSoft gRPC Demo")
            };
        }

        public static IEnumerable<ApiScope> GetScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("apiCore", "AdwardSoft API Core"),
                new ApiScope("apiPOS", "AdwardSoft API POS"),
                new ApiScope("apiIdentity", "AdwardSoft API Identity")
            };
        }
        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "Inside",
                    ClientName = "AdwardSoft Inside",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                   // RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret(configuration["Client:Inside:Secret"].Sha256())
                    },

                    AllowedScopes =
                    {
                        //IdentityServerConstants.StandardScopes.OpenId,
                       //  IdentityServerConstants.StandardScopes.Profile,
                        "apiCore", "apiPOS", "apiIdentity"
                    },

                    AllowOfflineAccess = true,

                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = 14400,
                    AccessTokenLifetime = 28800,

                    RedirectUris = {
                        configuration["Client:Inside:RedirectUris"].ToString()
                    },
                    PostLogoutRedirectUris = {
                        configuration["Client:Inside:PostLogoutRedirectUris"].ToString()
                    },
                },
                // Mobile
                new Client
                {
                    ClientId = "Mobile",
                    ClientName = "AdwardSoft Mobile",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    //RequireConsent = false,

                    //Used to retrieve the access token on the back channel.
                    ClientSecrets =
                    {
                        new Secret(configuration["Client:Mobile:Secret"].Sha256())
                    },

                    AllowedScopes =
                    {
                        "apiCore", "apiPOS", "apiIdentity"
                    },

                    AllowOfflineAccess = true,

                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    SlidingRefreshTokenLifetime = 14400,
                    AccessTokenLifetime = 28800
                }
            };
        }
    }
}
