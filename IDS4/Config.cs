using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Test;

namespace IDS4
{
    public static class Config
    {

        public static IEnumerable<IdentityResource> IdentityResources =>
          new[]
          {
             new IdentityResources.OpenId(),
             new IdentityResources.Phone(),
             new IdentityResources.Profile(),
             new IdentityResources.Email(),
             new IdentityResources.Address(),
             new IdentityResource
              {
                Name="role"
              }
           };

        public static IEnumerable<ApiScope> ApiScopes =>
        new[]
         {
             new ApiScope(   IdentityServerConstants.StandardScopes.OfflineAccess),
             new ApiScope("API")
        };




        public static IEnumerable<Client> Clients =>
                 new[]
                      {
                     new Client {
                     ClientId = "flutter",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "com.firedepartment.apps.flutter2:/oauth2redirect" },
                    PostLogoutRedirectUris={  "com.firedepartment.apps.flutter2:/oauth2redirect"},

                    AllowOfflineAccess=true,
                    AllowedScopes = {
                         IdentityServerConstants.StandardScopes.Profile,
                         IdentityServerConstants.StandardScopes.Email,
                               IdentityServerConstants.StandardScopes.Phone,
                         IdentityServerConstants.StandardScopes.OpenId,
                                   IdentityServerConstants.StandardScopes.Address,
                       IdentityServerConstants.StandardScopes.OfflineAccess,


                         "API",

                    },

                    RequireConsent = false,
                },
      };
    }
}