using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Bll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fodraszat.Web.Pages
{
    public class SzolgaltatasokModel : PageModel
    {
        private readonly SzolgaltatasService _szolgaltatasService;

        public SzolgaltatasokModel(SzolgaltatasService szolgaltatasService)
        {
            _szolgaltatasService = szolgaltatasService;
        }

        public List<SzolgaltatasModel> Szolgaltatasok { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Szolgaltatasok = await _szolgaltatasService.GetSzolgaltatasokAsync();

            return Page();
        }
    }
}
