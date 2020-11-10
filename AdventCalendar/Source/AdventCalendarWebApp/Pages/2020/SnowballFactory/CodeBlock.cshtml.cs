using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.SnowballFactory
{
    public class CodeBlockModel : PageModel
    {
        public bool CanSolve { get; }
        public bool Solved { get; private set; }
        [BindProperty]
        public int? Answer { get; set; }

        private const int door = 12;

        public CodeBlockModel(DayValidation dayValidation)
        {
            CanSolve = dayValidation.HasAccess(door);
        }

        public IActionResult OnGet(int? answer = null)
        {
            Answer = answer;
            if (!CanSolve ||
                !answer.HasValue)
            {
                return Page();
            }
            if (answer < 1000 || answer > 9999)
            {
                ModelState.AddModelError(string.Empty, "The pin is exactly four digits long, even the saboteur cannot change this.");
            }
            else if (answer == 5740)
            {
                Solved = true;
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"Entering {answer} does not open the gate. Try another answer.");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("CodeBlock", new { answer = Answer });
        }
    }
}