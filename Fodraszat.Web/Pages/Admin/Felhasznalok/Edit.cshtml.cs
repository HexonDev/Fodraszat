using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Bll;
using Fodraszat.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fodraszat.Web.Pages.Admin.Felhasznalok
{
    public class EditModel : PageModel
    {
        private readonly FelhasznaloService _felhasznaloService;
        private readonly RoleManager<IdentityRole<int>> _roleManager;


        public EditModel(FelhasznaloService felhasznaloService, RoleManager<IdentityRole<int>> roleManager)
        {
            _felhasznaloService = felhasznaloService;
            _roleManager = roleManager;
        }

        [BindProperty]
        public FelhasznaloModel Felhasznalo { get; set; }

        [BindProperty]
        [Display(Name = "Jogosultság")]
        public string SelectedRole { get; set; }
        public string[] CurrentRoles { get; set; }

        public List<SelectListItem> Roles { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await PopulatePage(id.Value);

            if (Felhasznalo == null)
            {
                return NotFound();
            }

            return Page();
        }



        public async Task<IActionResult> OnPostAdatokAsync()
        {
            CurrentRoles = await _felhasznaloService.GetFelhasznaloRolesAsync(Felhasznalo);
            Roles = _roleManager.Roles.Select(r => new SelectListItem(r.Name, r.Name)).ToList();

            if (ModelState.IsValid)
            {
                var result = await _felhasznaloService.EditFelhasznaloAsync(Felhasznalo);

                if (result.Succeeded)
                {
                    return RedirectToPage("./Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }


            return Page();

        }

        public async Task<IActionResult> OnPostRoleAddAsync()
        {
            await PopulatePage(Felhasznalo.Id);

            ModelState.Remove("Felhasznalo.Nev");
            ModelState.Remove("Felhasznalo.Email");
            ModelState.Remove("Felhasznalo.Telefonszam");
            ModelState.Remove("Felhasznalo.SzuletesiIdo");

            if (ModelState.IsValid)
            {
                var result = await _felhasznaloService.AddFelhasznaloToRoleAsync(Felhasznalo, SelectedRole);

                if (result.Succeeded)
                {
                    return RedirectToPage("./Edit", new { id = Felhasznalo.Id });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostRoleRemoveAsync()
        {
            await PopulatePage(Felhasznalo.Id);

            ModelState.Remove("Felhasznalo.Nev");
            ModelState.Remove("Felhasznalo.Email");
            ModelState.Remove("Felhasznalo.Telefonszam");
            ModelState.Remove("Felhasznalo.SzuletesiIdo");

            if (ModelState.IsValid)
            {
                var result = await _felhasznaloService.RemoveFelhasznaloFromRoleAsync(Felhasznalo, SelectedRole);

                if (result.Succeeded)
                {
                    return RedirectToPage("./Edit", new { id = Felhasznalo.Id });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }

        async Task PopulatePage(int? userId)
        {
            if(userId != null)
                Felhasznalo = await _felhasznaloService.GetFelhasznaloAsync(userId.Value);

            CurrentRoles = await _felhasznaloService.GetFelhasznaloRolesAsync(Felhasznalo);
            Roles = _roleManager.Roles.Select(r => new SelectListItem(r.Name, r.Name)).ToList();
        }
    }
}
