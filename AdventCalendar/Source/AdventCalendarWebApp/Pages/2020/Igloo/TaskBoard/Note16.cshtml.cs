using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard
{
    public class Note16Model : PageModel
    {
        private readonly DayValidation dayValidation;
        private const int door = 16;

        public Note16Model(DayValidation dayValidation)
        {
            this.dayValidation = dayValidation;
        }

        public bool Solved { get; set; } = false;

        [BindProperty]
        public string Answer { get; set; }

        public IActionResult OnGet(string answer)
        {
            if (!dayValidation.HasAccess(door))
            {
                return RedirectToPage("Index", new { invalidDoor = door });
            }
            if (string.IsNullOrEmpty(answer))
            {
                return Page();
            }
            if (string.Equals(answer, "food", StringComparison.InvariantCultureIgnoreCase))
            {
                Solved = true;
            }
            else
            {
                ModelState.AddModelError(string.Empty, $"No, {answer} is not the solution.");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Note16", new { answer = Answer });
        }
    }
}