using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Bll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Fodraszat.Web.Pages.Admin.Nyitvatartas
{
    public class EditModel : PageModel
    {
        private readonly NyitvatartasService _nyitvatartasService;

        public EditModel(NyitvatartasService nyitvatartasService)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _nyitvatartasService.EditNyitvatartasAsync(Nyitvatartas);   

            return RedirectToPage("./Index");
        }
    }
}
