using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Bll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fodraszat.Web.Pages
{
    public class FodraszokModel : PageModel
    {
        private readonly FodraszService _fodraszService;

        public FodraszokModel(FodraszService fodraszService)
        {
            _fodraszService = fodraszService;
        }

        public List<FodraszModel> Fodraszok { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Fodraszok = await _fodraszService.GetFodraszokAsync();

            return Page();
        }
    }
}
