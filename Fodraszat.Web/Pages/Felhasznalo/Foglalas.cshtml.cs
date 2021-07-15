using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Bll;
using Fodraszat.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fodraszat.Web.Pages.Felhasznalo
{
    public class FoglalasModel : PageModel
    {
        private readonly FoglalasService _foglalasService;
        private readonly FodraszService _fodraszService;
        private readonly SzolgaltatasService _szolgaltatasService;
        private readonly UserManager<Data.Entities.Felhasznalo> _userManager;

        public FoglalasModel(FoglalasService foglalasService, NyitvatartasService nyitvatartasService, FodraszService fodraszService, SzolgaltatasService szolgaltatasService, UserManager<Data.Entities.Felhasznalo> userManager)
        {
            _foglalasService = foglalasService;
            _fodraszService = fodraszService;
            _szolgaltatasService = szolgaltatasService;
            _userManager = userManager;
        }

        public List<DateTime> Idopontok { get; set; }

        public List<SelectListItem> Fodraszok { get; set; }
        public List<SelectListItem> Szolgaltatasok { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Fodrasz { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? Szolgaltatas { get; set; }

        [BindProperty(SupportsGet = true)]
        public byte Lepes { get; set; } = 1;
        [BindProperty]
        public DateTime Datum { get; set; }

        public IdopontModel Idopont { get; set; }
        public FodraszModel FodraszModel { get; set; }
        public SzolgaltatasModel SzolgaltatasModel { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (Fodrasz == null || Szolgaltatas == null)
                Lepes = 1;

            if (Lepes == 1)
            {
                Fodraszok = (await _fodraszService.GetFodraszokAsync()).Select(f => new SelectListItem(f.Nev, f.Id.ToString())).ToList();
                Szolgaltatasok = (await _szolgaltatasService.GetSzolgaltatasokAsync()).Select(sz => new SelectListItem($"{sz.Nev} - {sz.Ar} Ft" , sz.Id.ToString())).ToList();
            }else if (Lepes == 2)
            {

                if (Fodrasz != null && Szolgaltatas != null)
                {
                    FodraszModel = await _fodraszService.GetFodraszAsync(Fodrasz.Value);
                    SzolgaltatasModel = await _szolgaltatasService.GetSzolgaltatasAsync(Szolgaltatas.Value);

                    if (FodraszModel == null || SzolgaltatasModel == null)
                        return RedirectToPage("/Felhasznalo/Foglalas");

                    Idopontok = await _foglalasService.GetSzabadIdopontokAsync(Fodrasz.Value, Szolgaltatas.Value, DateTime.Now,
                        DateTime.Now.Date.AddDays(3));
                }


            }

            return Page();
        }

        public IActionResult OnPostFodraszAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Lepes = 2;

            return RedirectToPage("./Foglalas", new { lepes = Lepes, fodrasz = Fodrasz, szolgaltatas = Szolgaltatas });
        }

        public async Task<IActionResult> OnPostFoglalasAsync()
        {
            if (Fodrasz == null || Szolgaltatas == null)
                Lepes = 1;

            if (Szolgaltatas != null && Fodrasz != null)
                Idopont = new()
                {
                    FelhasznaloId = int.Parse(_userManager.GetUserId(User)),
                    SzolgaltatasId = Szolgaltatas.Value,
                    FodraszId = Fodrasz.Value,
                    Datum = Datum
                };

            var result = await _foglalasService.AddIdopontAsync(Idopont);

            return RedirectToPage("/Felhasznalo/Idopontok", new {sikeres = result});
        }
    }
}
