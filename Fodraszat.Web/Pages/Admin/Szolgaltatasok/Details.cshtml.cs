using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Fodraszat.Bll;

namespace Fodraszat.Web.Pages.Admin.Szolgaltatasok
{
    public class DetailsModel : PageModel
    {
        private readonly SzolgaltatasService _szolgaltatasService;

        public DetailsModel(SzolgaltatasService szolgaltatasService)
        {
            _szolgaltatasService = szolgaltatasService;
        }

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
    }
}
