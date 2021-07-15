using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Bll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fodraszat.Web.Pages.Admin.Szolgaltatasok
{
    public class IndexModel : PageModel
    {
        private readonly SzolgaltatasService _szolgaltatasService;


        public IndexModel(SzolgaltatasService szolgaltatasService)
        {
            _szolgaltatasService = szolgaltatasService;
        }

        public IList<SzolgaltatasModel> Szolgaltatasok { get;set; }

        public async Task OnGetAsync()
        {
            Szolgaltatasok = await _szolgaltatasService.GetSzolgaltatasokAsync();
        }
    }
}
