// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.



using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServerHost.Quickstart.UI
{

    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly IIdentityServerInteractionService _interactionService;
        private readonly UserManager<IdentityUser> _userManager;
        public AccountController(
             UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IIdentityServerInteractionService interactionService)
        {
            _signInManager = signInManager;

            _interactionService = interactionService;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (TempData["Status"] !=null) {
                ModelState.AddModelError("", "帳號密碼錯誤");
                TempData["Status"] = null;
            }

            var externalProviders = await _signInManager.GetExternalAuthenticationSchemesAsync();

            
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalProviders = externalProviders
            });

          
        }


        //[Route("localApi")]
        //[Authorize(LocalApi.PolicyName)]
        //public IActionResult test() {

        //    return Ok("hi");
        //}



        public async Task<IActionResult> userinfobyid(string userid)
        {
            var user = await _userManager.FindByIdAsync(userid);
            var claims = await _userManager.GetClaimsAsync(user);

            var map = claims.ToDictionary(x => x.Type, x => x.Value);

            var json = JsonSerializer.Serialize(map);

            return Ok(json);
        }


        public  async Task<IActionResult>  getuserinfo(string userid) {//identityserver 函數

            var user = await _userManager.FindByIdAsync(userid);
            var claims = await _userManager.GetClaimsAsync(user);

            var map = claims.ToDictionary(x => x.Type, x => x.Value);
            
        var json =    JsonSerializer.Serialize(map);

            return Ok(json);

        }


   



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm, string button)
        {

        
            if (button != "login")
            {
                var context = await _interactionService.GetAuthorizationContextAsync(vm.ReturnUrl);
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interactionService.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (context.IsNativeClient())
                    {
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage("Redirect", vm.ReturnUrl);
                    }

                    return Redirect(vm.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

       

                var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, false, false);
              
                if (result.Succeeded)
                {

                return Redirect(vm.ReturnUrl);
            }
                else
                {
                TempData["Status"] = "fail";
                return RedirectToAction("Login","Account", new { returnUrl = vm.ReturnUrl });
                }
            

     
        }


        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

            if (string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
            {
                return RedirectToAction("Index", "Home");
            }

       return Ok("正在登出 請稍後");
        }


            [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
