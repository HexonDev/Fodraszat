using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fodraszat.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Felhasznalo> _userManager;
        private readonly SignInManager<Felhasznalo> _signInManager;

        public IndexModel(
            UserManager<Felhasznalo> userManager,
            SignInManager<Felhasznalo> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Display(Name = "Felhasználónév")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Telefonszám")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Név")]
            public string Nev { get; set; }

            [Display(Name = "Születési idő")]
            public DateTime SzuletesiIdo { get; set; }
        }

        private async Task LoadAsync(Felhasznalo user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var nev = user.Nev;
            var szuletesiIdo = user.SzuletesiIdo;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Nev = nev,
                SzuletesiIdo = szuletesiIdo
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber || Input.SzuletesiIdo != user.SzuletesiIdo || Input.Nev != user.Nev)
            {
                user.PhoneNumber = Input.PhoneNumber;
                user.Nev = Input.Nev;
                user.SzuletesiIdo = Input.SzuletesiIdo;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
