using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Bll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fodraszat.Web.Pages.Admin.Szolgaltatasok
{
    public class EditModel : PageModel
    {
        private readonly SzolgaltatasService _szolgaltatasService;

        public EditModel(SzolgaltatasService szolgaltatasService)
        {
            _szolgaltatasService = szolgaltatasService;
        }

        [BindProperty]
        public SzolgaltatasModel Szolgaltatas { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Szolgaltatas = await _szolgaltatasService.GetSzolgaltatasAsync(id.Value);

            if (Szolgaltatas == null)
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

            await _szolgaltatasService.EditSzolgaltatasAsync(Szolgaltatas);   

            return RedirectToPage("./Index");
        }
    }
}
