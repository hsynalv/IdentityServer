using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer.AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources => new ApiResource[]
        {
            new ApiResource("resource_api1")
            { 
                Scopes = {"api1.read", "api1.write", "api1.update" },
                ApiSecrets = new[]{new Secret("secretApi1".Sha512())},  // Basic Auth Doğrulama için 
            },
            new ApiResource("resource_api2")
            { 
                Scopes = {"api2.read", "api2.write", "api2.update" },
                ApiSecrets = new[]{new Secret("secretApi2".Sha512())},  // Basic Auth Doğrulama için 
            }
        };

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>()
            {
                new ApiScope("api1.read","API 1 için okuma izni"),
                new ApiScope("api1.write","API 1 için write izni"),
                new ApiScope("api1.update","API 1 için update izni"),
                new ApiScope("api2.read","API 2 için okuma izni"),
                new ApiScope("api2.write","API 2 için write izni"),
                new ApiScope("api2.update","API 2 için update izni")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client
                {
                    ClientId = "Client1",
                    ClientName = "Client 1 web uygulaması",
                    ClientSecrets = new []{new Secret("Secret1".Sha512())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api1.read" }
                },

                new Client
                {
                    ClientId = "Client2",
                    ClientName = "Client 2 web uygulaması",
                    ClientSecrets = new []{new Secret("Secret2".Sha512())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api2.read", "api2.write", "api2.update" }
                }
            };
        }
    }
}
