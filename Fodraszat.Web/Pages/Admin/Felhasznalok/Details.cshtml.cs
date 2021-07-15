using System.Threading.Tasks;
using Fodraszat.Bll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fodraszat.Web.Pages.Admin.Felhasznalok
{
    public class DetailsModel : PageModel
    {
        private readonly FelhasznaloService _felhasznaloService;

        public DetailsModel(FelhasznaloService felhasznaloService)
        {
            _felhasznaloService = felhasznaloService;
        }

        public FelhasznaloModel Felhasznalo { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Felhasznalo = await _felhasznaloService.GetFelhasznaloAsync(id.Value);

            if (Felhasznalo == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
