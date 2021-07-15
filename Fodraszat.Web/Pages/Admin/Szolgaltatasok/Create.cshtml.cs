using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Bll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fodraszat.Web.Pages.Admin.Szolgaltatasok
{
    public class CreateModel : PageModel
    {
        private readonly SzolgaltatasService _szolgaltatasService;

        public CreateModel(SzolgaltatasService szolgaltatasService)
        {
            _szolgaltatasService = szolgaltatasService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SzolgaltatasModel Szolgaltatas { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _szolgaltatasService.AddSzolgaltatasAsync(Szolgaltatas);

            return RedirectToPage("./Index");
        }
    }
}
