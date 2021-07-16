using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Data.Entities;
using Fodraszat.Web.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Fodraszat.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Felhasznalo> _userManager;
        private readonly SignInManager<Felhasznalo> _signInManager;
        private readonly IWebHostEnvironment _environment;
        private readonly ImageUploadSettings _imageUploadSettings;

        public IndexModel(
            UserManager<Felhasznalo> userManager,
            SignInManager<Felhasznalo> signInManager,
            IWebHostEnvironment environment,
            IOptions<ImageUploadSettings> imageUploadSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
            _imageUploadSettings = imageUploadSettings.Value;
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

            [Display(Name ="Profilkép")]
            public IFormFile Profilkep { get; set; }

            public string? ProfilkepUtvonal { get; set; }
        }

        private async Task LoadAsync(Felhasznalo user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var nev = user.Nev;
            var szuletesiIdo = user.SzuletesiIdo;
            var profilkep = user.Profilkep;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Nev = nev,
                SzuletesiIdo = szuletesiIdo,
                ProfilkepUtvonal = profilkep
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

            if (Input?.Profilkep != null && Input.Profilkep.Length > 0)
            {
                var fileName = Input.Profilkep.FileName;
                var fileExt = Path.GetExtension(fileName).ToLowerInvariant();

                if (string.IsNullOrEmpty(fileExt) || !_imageUploadSettings.AllowedExtensions.Contains(fileExt))     
                {
                    ModelState.AddModelError("Input.Profilkep", "A kép kiterjesztése nem megfelelő");
                    await LoadAsync(user);
                    return Page();
                }

                if (Input.Profilkep.Length > _imageUploadSettings.MaxFileSize)
                {
                    ModelState.AddModelError("Input.Profilkep", $"A kép mérete túl nagy ({(Input.Profilkep.Length / 1024f / 1024f):0.00} MB)! Maximális méret: {_imageUploadSettings.MaxFileSize / 1024f / 1024f} MB");
                    await LoadAsync(user);
                    return Page();
                }

                var filePath = $"{user.Id}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}{fileExt}";
                var targetDirectory = "profile";
                var fullPath = Path.Combine(_environment.WebRootPath, targetDirectory, filePath);

                await using (var stream = System.IO.File.Create(fullPath))
                {
                    await Input.Profilkep.CopyToAsync(stream);
                }

                user.Profilkep = filePath;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    StatusMessage = "Váratlan hiba törént a kép feltöltése közben.";
                    return RedirectToPage();
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input != null && (Input.PhoneNumber != phoneNumber || Input.SzuletesiIdo != user.SzuletesiIdo || Input.Nev != user.Nev))
            {
                user.PhoneNumber = Input.PhoneNumber;
                user.Nev = Input.Nev;
                user.SzuletesiIdo = Input.SzuletesiIdo;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    StatusMessage = "Váratlan hiba törént az adatok frissítése közben.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Sikeresen frissítetted a profilodat!";
            return RedirectToPage();
        }
    }
}
