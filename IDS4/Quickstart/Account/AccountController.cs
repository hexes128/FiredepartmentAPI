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
            if (TempData["Status"] != null)
            {
                ModelState.AddModelError("", "±b¸¹±K½X¿ù»~");
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


        public async Task<IActionResult> getuserinfo(string userid)
        {//identityserver ¨ç¼Æ

            var user = await _userManager.FindByIdAsync(userid);
            var claims = await _userManager.GetClaimsAsync(user);

            var map = claims.ToDictionary(x => x.Type, x => x.Value);

            var json = JsonSerializer.Serialize(map);

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
                    await _interactionService.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                    if (context.IsNativeClient())
                    {
                        return this.LoadingPage("Redirect", vm.ReturnUrl);
                    }
                    return Redirect(vm.ReturnUrl);
                }
                else
                {
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
                return RedirectToAction("Login", "Account", new { returnUrl = vm.ReturnUrl });
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

            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
