using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Hideout
{
    public class NoteModel : PageModel
    {
        private readonly DayValidation dayValidation;
        private const int door = 17;

        public NoteModel(DayValidation dayValidation)
        {
            this.dayValidation = dayValidation;
        }

        public IActionResult OnGet()
        {
            if (!dayValidation.HasAccess(door))
            {
                return NotFound();
            }
            return Page();
        }
    }
}