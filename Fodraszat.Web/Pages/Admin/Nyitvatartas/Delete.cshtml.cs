using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Bll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fodraszat.Web.Pages.Admin.Nyitvatartas
{
    public class DeleteModel : PageModel
    {
        private readonly NyitvatartasService _nyitvatartasService;


        public DeleteModel(NyitvatartasService nyitvatartasService)
        {
            _nyitvatartasService = nyitvatartasService;
        }

        [BindProperty]
        public NyitvatartasModel Nyitvatartas { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Nyitvatartas = await _nyitvatartasService.GetNyitvatartasByIdAsync(id.Value);

            if (Nyitvatartas == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Nyitvatartas = await _nyitvatartasService.GetNyitvatartasByIdAsync(id.Value);
            await _nyitvatartasService.DeleteNyitvatartasAsync(Nyitvatartas);

            return RedirectToPage("./Index");
        }
    }
}
