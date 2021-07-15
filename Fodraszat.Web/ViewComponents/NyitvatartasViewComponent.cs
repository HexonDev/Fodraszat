
using Fodraszat.Bll;
using Fodraszat.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fodraszat.Web.ViewComponents
{
    public class NyitvatartasViewComponent : ViewComponent
    {
        private readonly NyitvatartasService _nyitvatartasService;

        public NyitvatartasViewComponent(NyitvatartasService nyitvatartasService)
        {
            _nyitvatartasService = nyitvatartasService;
        }

        public async Task<IViewComponentResult> InvokeAsync(DateTime mettol, DateTime meddig, int? db = null )
        {
            var nyitvatartas = await _nyitvatartasService.GetNyitvatartasAsync(mettol, meddig, db);

            var viewModel = new NyitvatartasViewModel
            {
                Nyitvatartas = nyitvatartas
            };

            return View(viewModel);
        }
    }
}
