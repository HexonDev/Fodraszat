using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Bll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fodraszat.Web.Pages.Admin.Nyitvatartas
{
    public class CreateModel : PageModel
    {
        private readonly NyitvatartasService _nyitvatartasService;

        public CreateModel(NyitvatartasService nyitvatartasService)
        {
            _nyitvatartasService = nyitvatartasService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public NyitvatartasModel Nyitvatartas { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _nyitvatartasService.AddNyitvatartasAsync(Nyitvatartas);

            return RedirectToPage("./Index");
        }
    }
}
