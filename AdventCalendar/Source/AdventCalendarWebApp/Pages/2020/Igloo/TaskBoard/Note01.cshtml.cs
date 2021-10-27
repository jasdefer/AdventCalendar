using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard
{
    public class Note01Model : PageModel
    {
        private const int door = 1;
        private readonly DayValidation dayValidation;

        public Note01Model(DayValidation dayValidation)
        {
            this.dayValidation = dayValidation;
        }
        public IActionResult OnGet()
        {
            var hasAccess = dayValidation.HasAccess2020(door);
            if (!hasAccess)
            {
                return RedirectToPage("Index", new { invalidDoor = door });
            }
            return Page();
        }
    }
}