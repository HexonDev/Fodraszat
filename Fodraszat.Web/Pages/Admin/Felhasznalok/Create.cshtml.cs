using System.Threading.Tasks;
using Fodraszat.Bll;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fodraszat.Web.Pages.Admin.Felhasznalok
{
    public class CreateModel : PageModel
    {
        private readonly FelhasznaloService _felhasznaloService;

        public CreateModel(FelhasznaloService felhasznaloService)
        {
            _felhasznaloService = felhasznaloService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public RegisztracioModel RegisztracioModel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _felhasznaloService.AddFelhasznaloAsync(RegisztracioModel);

                if (result.Succeeded)
                {
                    return RedirectToPage("./Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
