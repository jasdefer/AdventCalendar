using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.EastWing
{
    public class TopDrawerModel : PageModel
    {
        private readonly DayValidation dayValidation;

        [BindProperty]
        public int? FirstDigit { get; set; }
        [BindProperty]
        public int? SecondDigit { get; set; }
        [BindProperty]
        public int? ThirdDigit { get; set; }

        public TopDrawerModel(DayValidation dayValidation)
        {
            this.dayValidation = dayValidation;
        }


        public IActionResult OnGet(int? firstDigit, int? secondDigit, int? thirdDigit)
        {
            FirstDigit = firstDigit;
            SecondDigit = secondDigit;
            ThirdDigit = thirdDigit;

            if (!FirstDigit.HasValue &&
                !SecondDigit.HasValue &&
                !ThirdDigit.HasValue)
            {
                return Page();
            }
            if (!dayValidation.HasAccess(1))
            {
                ModelState.AddModelError(string.Empty, "Some magic inside the lock prevents it to be opened before the correct time has come.");
                return Page();
            }
            if (FirstDigit == 1 &&
                SecondDigit == 9 &&
                ThirdDigit == 9)
            {
                return RedirectToPage("Index", new { solvedTheRiddle = true });
            }
            ModelState.AddModelError(string.Empty, "Code invalid");
            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("TopDrawer", new { FirstDigit, SecondDigit, ThirdDigit });
        }
    }
}