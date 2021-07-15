using System.Threading.Tasks;
using Fodraszat.Bll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fodraszat.Web.Pages.Admin.Felhasznalok
{
    public class DeleteModel : PageModel
    {
        private readonly FelhasznaloService _felhasznaloService;

        public DeleteModel(FelhasznaloService felhasznaloService)
        {
            _felhasznaloService = felhasznaloService;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Felhasznalo = await _felhasznaloService.GetFelhasznaloAsync(id.Value);

            if (Felhasznalo != null)
            {
                await _felhasznaloService.DeleteFelhasznaloAsync(Felhasznalo);
            }

            return RedirectToPage("./Index");
        }
    }
}
