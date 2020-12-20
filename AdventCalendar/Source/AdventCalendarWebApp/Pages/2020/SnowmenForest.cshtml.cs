using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020
{
    public class SnowmenForestModel : PageModel
    {
        private static readonly int[] sequence = new int[10] { 5, 2, 0, 9, 4, 8, 7, 6, 1, 3 };
        private readonly DayValidation dayValidation;
        private const int door = 22;

        public bool CanSolve { get; set; }
        public bool Solved { get; set; }
        public int CurrentElfPosition { get; set; }

        public SnowmenForestModel(DayValidation dayValidation)
        {
            this.dayValidation = dayValidation;
        }

        public IActionResult OnGet(int tree, int guess)
        {
            CanSolve = dayValidation.HasAccess(door);
            if (!CanSolve)
            {
                return Page();
            }
            guess++;
            if (guess >= sequence.Length)
            {
                guess = 0;
            }
            ViewData["guess"] = guess;
            CurrentElfPosition = sequence[guess];
            if (tree == CurrentElfPosition)
            {
                Solved = true;
            }
            return Page();
        }
    }
}
