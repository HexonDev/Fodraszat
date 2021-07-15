using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Bll;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fodraszat.Web.Pages.Fodrasz
{
    public class IdopontokModel : PageModel
    {
        private readonly FelhasznaloService _felhasznaloService;
        private readonly FoglalasService _foglalasService;
        private readonly UserManager<Data.Entities.Felhasznalo> _userManager;

        public IdopontokModel(FelhasznaloService felhasznaloService, FoglalasService foglalasService, UserManager<Data.Entities.Felhasznalo> userManager)
        {
            _felhasznaloService = felhasznaloService;
            _foglalasService = foglalasService;
            _userManager = userManager;
        }

        public List<IdopontModel> Idopontok { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var felhasznaloId = int.Parse(_userManager.GetUserId(User));
            Idopontok = (await _foglalasService.GetIdopontokAsync(felhasznaloId)).OrderByDescending(i => i.Datum).ToList();

            return Page();
        }
    }
}
