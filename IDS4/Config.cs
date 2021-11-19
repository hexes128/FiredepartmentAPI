﻿using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Test;

namespace IDS4
{
    public static class Config //2019影片提供的模板
    {
        public static List<TestUser> Users
        {
            get
            {
                var address = new
                {
                    street_address = "One Hacker Way",
                    locality = "Heidelberg",
                    postal_code = 69118,
                    country = "Germany"
                };

                return new List<TestUser>
        {
          new TestUser
          {
            SubjectId = "818727",
            Username = "alice",
            Password = "alice",
            Claims =
            {
              new Claim(JwtClaimTypes.Name, "Alice Smith"),
              new Claim(JwtClaimTypes.GivenName, "Alice"),
              new Claim(JwtClaimTypes.FamilyName, "Smith"),
              new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
              new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
              new Claim(JwtClaimTypes.Role, "admin"),
              new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
              new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address),
                IdentityServerConstants.ClaimValueTypes.Json)
            }
          },
          new TestUser
          {
            SubjectId = "88421113",
            Username = "bob",
            Password = "bob",
            Claims =
            {
              new Claim(JwtClaimTypes.Name, "Bob Smith"),
              new Claim(JwtClaimTypes.GivenName, "Bob"),
              new Claim(JwtClaimTypes.FamilyName, "Smith"),
              new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
              new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
              new Claim(JwtClaimTypes.Role, "user"),
              new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
              new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address),
                IdentityServerConstants.ClaimValueTypes.Json)
            }
          }
        };
            }
        }

        public static IEnumerable<IdentityResource> IdentityResources =>
          new[]
          {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email(),
        new IdentityResource
        {
          Name = "role",
          UserClaims = new List<string> {"role"}
        }
          };

        public static IEnumerable<ApiScope> ApiScopes =>
          new[]
          {
             new ApiScope(   IdentityServerConstants.StandardScopes.OfflineAccess),

          new ApiScope("API"),
              new ApiScope(IdentityServerConstants.LocalApi.ScopeName),
          };




        public static IEnumerable<Client> Clients =>
      new[]
      {
        //// m2m client credentials flow client
        //new Client
        //{
        //  ClientId = "m2m.client",
        //  ClientName = "Client Credentials Client",

        //  AllowedGrantTypes = GrantTypes.ClientCredentials,
        //  ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},

        //  AllowedScopes = {"weatherapi.read", "weatherapi.write"}
        //},

        //// interactive client using code flow + pkce
        //new Client
        //{
        //  ClientId = "interactive",
        //  ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},

        //  AllowedGrantTypes = GrantTypes.Code,

        //  RedirectUris = {"https://localhost:5444/signin-oidc"},
        //  FrontChannelLogoutUri = "https://localhost:5444/signout-oidc",
        //  PostLogoutRedirectUris = {"https://localhost:5444/signout-callback-oidc"},

        //  AllowOfflineAccess = true,
        //  AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, "API"},
        //  RequirePkce = true,
        //  RequireConsent = true,
        //  AllowPlainTextPkce = false
        //},


        new Client // Config 註冊MVC 客戶
        {
          ClientId = "MVC_Client",
          ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},

          AllowedGrantTypes = GrantTypes.Code,
         
          RedirectUris = {"https://localhost:44327/signin-oidc" },
        
          PostLogoutRedirectUris = {"https://localhost:44327/signout-callback-oidc"},
          AllowOfflineAccess=true,

          AllowedScopes = {    IdentityServerConstants.StandardScopes.Profile,
                         IdentityServerConstants.StandardScopes.Email,
                         IdentityServerConstants.StandardScopes.OpenId,
                         IdentityServerConstants.LocalApi.ScopeName ,
                         "API",},

          RequireConsent = false,

        },
                  new Client {
                    ClientId = "flutter",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "http://localhost:4000/" },
                    AllowedCorsOrigins = { "http://localhost:4000" },
                    AllowOfflineAccess=true,
                    AllowedScopes = {
                         IdentityServerConstants.StandardScopes.Profile,
                         IdentityServerConstants.StandardScopes.Email,
                         IdentityServerConstants.StandardScopes.OpenId,
                          IdentityServerConstants.StandardScopes.OfflineAccess,
                         IdentityServerConstants.LocalApi.ScopeName ,
                IdentityServerConstants.StandardScopes.OfflineAccess,
                         "API",

                    },

                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                },

                       new Client {
                    ClientId = "flutter2",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "io.identityserver.demo" },
                  
                    AllowOfflineAccess=true,
                    AllowedScopes = {
                         IdentityServerConstants.StandardScopes.Profile,
                         IdentityServerConstants.StandardScopes.Email,
                         IdentityServerConstants.StandardScopes.OpenId,
                       
                         IdentityServerConstants.LocalApi.ScopeName ,

                         "API",

                    },

                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                },



      };
    }
}