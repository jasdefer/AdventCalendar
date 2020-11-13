using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Hideout
{
    public class IndexModel : PageModel
    {
        public bool Rejected { get; private set; }
        public IActionResult OnGet(bool? rejected)
        {
            Rejected = rejected == true;
            return Page();
        }
    }
}