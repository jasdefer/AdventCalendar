using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard
{
    public class Note07Model : PageModel
    {
        private readonly DayValidation dayValidation;
        private const int door = 7;

        public Note07Model(DayValidation dayValidation)
        {
            this.dayValidation = dayValidation;
        }

        public bool Solved { get; set; } = false;


        [BindProperty]
        public int? Answer { get; set; } = null;

        public IActionResult OnGet(int? answer)
        {
            if (!dayValidation.HasAccess(door))
            {
                return RedirectToPage("Index", new { invalidDoor = door });
            }
            if (!answer.HasValue)
            {
                return Page();
            }
            if (answer == 32)
            {
                Solved = true;
            }
            else if (answer == 25)
            {
                ModelState.AddModelError(string.Empty, $"Hm, {answer} is a good try, but it is too obvious. Aren't there more digits of the note?");
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"No {answer} is incorrect.");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Note07", new { answer = Answer });
        }
    }
}