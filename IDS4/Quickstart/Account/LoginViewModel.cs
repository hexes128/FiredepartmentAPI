// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IdentityServerHost.Quickstart.UI
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="�b�����i�ť�")]
        public string Username { get; set; }
        [Required(ErrorMessage = "�K�X���i�ť�")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }

        public IEnumerable<AuthenticationScheme> ExternalProviders { get; set; }
    }
}