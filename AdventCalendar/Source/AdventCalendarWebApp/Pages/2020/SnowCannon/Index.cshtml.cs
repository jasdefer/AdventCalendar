using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.SnowCannon
{
    public class IndexModel : PageModel
    {
        private static readonly bool[] CorrectSolution = new bool[]
        {
            false,
            true,
            true,
            false,
            true,
            true
        };

        public bool CanSolve { get; }
        public bool Solved { get; private set; }

        [BindProperty]
        public bool Answer1 { get; set; }
        [BindProperty]
        public bool Answer2 { get; set; }
        [BindProperty]
        public bool Answer3 { get; set; }
        [BindProperty]
        public bool Answer4 { get; set; }
        [BindProperty]
        public bool Answer5 { get; set; }
        [BindProperty]
        public bool Answer6 { get; set; }

        private const int door = 13;

        public IndexModel(DayValidation dayValidation)
        {
            CanSolve = dayValidation.HasAccess2020(door);
        }

        public IActionResult OnGet(bool[] answer)
        {
            if (!CanSolve)
            {
                return Page();
            }

            if (answer == null ||
                answer.Length != 6)
            {
                return Page();
            }

            Answer1 = answer[0];
            Answer2 = answer[1];
            Answer3 = answer[2];
            Answer4 = answer[3];
            Answer5 = answer[4];
            Answer6 = answer[5];
            Solved = IsCorrect(answer);
            if (!Solved)
            {
                ModelState.AddModelError(string.Empty, $"The snow cannon does not start. Try flipping some switches.");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Index", new { answer = new bool[] { Answer1, Answer2, Answer3, Answer4, Answer5, Answer6 } });
        }

        private static bool IsCorrect(bool[] answer)
        {
            for (int i = 0; i < CorrectSolution.Length; i++)
            {
                if (answer[i] != CorrectSolution[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}