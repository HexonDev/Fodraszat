using Fodraszat.Bll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fodraszat.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly FodraszService _fodraszService;

        public IndexModel(FodraszService fodraszService)
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
