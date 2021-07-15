using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fodraszat.Bll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace Fodraszat.Web.Pages.Admin.Nyitvatartas
{
    public class IndexModel : PageModel
    {
        private readonly NyitvatartasService _nyitvatartasService;

        public IndexModel(NyitvatartasService nyitvatartasService)
        {
            _nyitvatartasService = nyitvatartasService;
        }

        public IList<NyitvatartasModel> Nyitvatartas { get;set; }

        public async Task OnGetAsync()
        {
            Nyitvatartas = await _nyitvatartasService.GetNyitvatartasAsync(DateTime.Today, DateTime.Today.AddYears(1));
        }
    }
}
