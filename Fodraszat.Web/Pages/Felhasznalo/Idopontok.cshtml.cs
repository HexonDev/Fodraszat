using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Bll;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fodraszat.Web.Pages.Felhasznalo
{
    public class IdopontokModel : PageModel
    {
        private readonly FoglalasService _foglalasService;
        private readonly UserManager<Data.Entities.Felhasznalo> _userManager;

        public IdopontokModel(FoglalasService foglalasService, UserManager<Data.Entities.Felhasznalo> userManager)
        {
            _foglalasService = foglalasService;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)] 
        public bool? Sikeres { get; set; } = null;
        public List<IdopontModel> Idopontok { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var felhasznaloId = int.Parse(_userManager.GetUserId(User));
            Idopontok = (await _foglalasService.GetIdopontokAsync(null, felhasznaloId)).OrderByDescending(i => i.Datum).ToList();

            return Page();
        }
    }
}
