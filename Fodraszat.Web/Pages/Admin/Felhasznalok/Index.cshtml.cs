using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Bll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fodraszat.Web.Pages.Felhasznalok
{
    public class IndexModel : PageModel
    {
        private readonly FelhasznaloService _felhasznaloService;

        public IndexModel(FelhasznaloService felhasznaloService)
        {
            _felhasznaloService = felhasznaloService;
        }

        public IList<FelhasznaloModel> Felhasznalok { get;set; }

        public async Task OnGetAsync()
        {
            Felhasznalok = await _felhasznaloService.GetFelhasznalokAsync();
        }
    }
}
