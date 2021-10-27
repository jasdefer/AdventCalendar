using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard
{
    public class Note05Model : PageModel
    {
        private readonly DayValidation dayValidation;
        private const int door = 5;
        private static readonly string[] listedWords = new string[]
        {
            "backup",
            "weaken",
            "bar",
            "bus",
            "cars",
            "dense",
            "game",
            "tinker"
        };

        public Note05Model(DayValidation dayValidation)
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
            if (CompareHelper.AreEqual("bank", answer))
            {
                Solved = true;
            }
            else if (CompareHelper.Contains(listedWords, answer))
            {
                ModelState.AddModelError(string.Empty, $"Just copying a single word from the list wont solve this riddle.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"No, {answer} cannot be the solution.");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Note05", new { answer = Answer });
        }
    }
}