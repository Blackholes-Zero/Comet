using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using SanFu.PassPort.AppSetts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SanFu.PassPort.IdentityConfig
{
    public class Configs
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources(IdentityServer identityServer)
        {
            var apis = identityServer.ApiResources?.Select(p => new ApiResource(p.Name, p.DisplayName));
            return apis;
        }

        public static IEnumerable<Client> GetClients(IdentityServer identityServer)
        {
            var scopes = new List<string> {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile
            };

            identityServer.Clients.ForEach(p =>
            {
                if (!scopes.Contains(p.ApiResourceName))
                {
                    scopes.Add(p.ApiResourceName);
                }
            });

            var clients = identityServer.Clients?.Where(p => p.GrantType.ToLower().Equals("implicit")).Select(p => new Client
            {
                ClientId = p.Id,
                ClientName = p.Name,
                //ClientSecrets = { new Secret(p.Secret.Sha256()) },
                AllowedGrantTypes = GrantTypes.Implicit,
                RequireConsent = false,
                RedirectUris = { p.RedirectUrl },
                PostLogoutRedirectUris = new string[] { p.LogoutRedirectUrl },
                AllowedCorsOrigins = new string[] { p.AllowedCorsOrigins },
                AllowedScopes = scopes,
                AllowOfflineAccess = true,
                AllowAccessTokensViaBrowser = true,
            });

            var clients1 = identityServer.Clients?.Where(p => p.GrantType.ToLower().Equals("resourceownerpasswordandclientcredentials"))?.Select(p => new Client
            {
                ClientId = p.Id,
                ClientName = p.Name,
                ClientSecrets = { new Secret(p.Secret?.Sha256()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireConsent = false,
                //RedirectUris = { p.RedirectUrl },
                //PostLogoutRedirectUris = new string[] { p.LogoutRedirectUrl },
                AllowedCorsOrigins = new string[] { p.AllowedCorsOrigins },
                AllowedScopes = scopes,
                AllowOfflineAccess = true,
                AllowAccessTokensViaBrowser = true,
            });
            if (clients1.Any())
            {
                var cli = clients.Union(clients1);
                return cli;
            }
            return clients;
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "admin",
                    Password = "admin",
                    Claims = new List<Claim>
                    {
                        new Claim("name", "admin"),
                        new Claim("website", "https://alice.com")
                    }
                }
            };
        }
    }
}