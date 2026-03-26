// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using GxWapi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace GxWapi.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterSuccessModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterSuccessModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            StatusMessage = "Thank you. Votre enregistrement a REUSSI, repondez au email qui vous a ete envoye pour VALIDATION de votre compte.";
            return Page();
        }
    }
}
