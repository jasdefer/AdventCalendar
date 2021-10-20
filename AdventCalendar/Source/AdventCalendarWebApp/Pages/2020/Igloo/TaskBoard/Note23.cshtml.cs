using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard
{
    public class Note23Model : PageModel
    {
        private readonly DayValidation dayValidation;
        private const int door = 23;

        public Note23Model(DayValidation dayValidation)
        {
            this.dayValidation = dayValidation;
        }

        public bool Solved { get; set; } = false;

        [BindProperty]
        public string Answer { get; set; }
        public IActionResult OnGet(string answer)
        {
            if (!dayValidation.HasAccess2020(door))
            {
                return RedirectToPage("Index", new { invalidDoor = door });
            }
            if (string.IsNullOrEmpty(answer))
            {
                return Page();
            }
            if (CompareHelper.AreEqual(answer, "otis"))
            {
                Solved = true;
            }
            else if (CompareHelper.AreEqual(answer, "sito"))
            {
                ModelState.AddModelError(string.Empty, $"No, {answer} is wrong, what about the item above the numbers?");
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"No, {answer} is not the saboteur.");
            }
            return Page();

        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Note23", new { answer = Answer });
        }
    }
}
