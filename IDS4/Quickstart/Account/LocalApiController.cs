using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IDS4.Quickstart.Account
{
    [Route("localApi")]
    [Authorize(LocalApi.PolicyName)]
    public class LocalApiController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public LocalApiController(    UserManager<IdentityUser> _userManager) {
            userManager = _userManager;
        }
        public async Task< IActionResult> Get(string userid)
        {

            var user = await userManager.FindByIdAsync(userid);
            var claims = await userManager.GetClaimsAsync(user);

            var map = claims.ToDictionary(x => x.Type, x => x.Value);

            var json = JsonSerializer.Serialize(map);

            return Ok(json);
        }


    }
}
