using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard
{
    public class Note14Model : PageModel
    {
        private readonly DayValidation dayValidation;
        private const int door = 14;

        public Note14Model(DayValidation dayValidation)
        {
            this.dayValidation = dayValidation;
        }

        public IActionResult OnGet()
        {
            if (!dayValidation.HasAccess(door))
            {
                return RedirectToPage("Index", new { invalidDoor = door });
            }
            return Page();
        }
    }
}