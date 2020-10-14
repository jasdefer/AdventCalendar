using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard
{
    public class Note09Model : PageModel
    {
        private readonly DayValidation dayValidation;
        private const int door = 9;
        private static readonly string[] Toys = new string[]
        {
            "Ball",
            "Train",
            "Puzzle",
            "Trumpet",
            "Car",
            "Duck",
            "Elefant",
            "Helicopter",
            "Snake"
        };

        public Note09Model(DayValidation dayValidation)
        {
            this.dayValidation = dayValidation;
        }

        public bool Solved { get; set; } = false;
        public List<SelectListItem> SelectableToys { get; private set; }

        [BindProperty]
        public string[] SelectedToys { get; set; }

        public IActionResult OnGet(string[] answer)
        {
            if (!dayValidation.HasAccess(door))
            {
                return RedirectToPage("Index", new { invalidDoor = door });
            }
            SetupSelectableToys();
            if (answer == null || answer.Length == 0)
            {
                return Page();
            }
            var isBadRequest = IsBadRequest(answer);
            if (isBadRequest)
            {
                return BadRequest();
            }
            var isCorrectAnswer = IsCorrectAnswer(answer);
            Solved = isCorrectAnswer;
            if (!isCorrectAnswer)
            {
                ModelState.AddModelError(string.Empty, $"Your answer ({string.Join(", ",answer)}) is false.");
            }
            SelectedToys = answer;

            return Page();
        }

        private bool IsCorrectAnswer(string[] answer)
        {
            return answer[0] == Toys[6] &&
                answer[1] == Toys[1] &&
                answer[2] == Toys[0];
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Note09", new { answer = SelectedToys });
        }

        private void SetupSelectableToys()
        {
            SelectableToys = new List<SelectListItem>();
            for (int i = 0; i < Toys.Length; i++)
            {
                var item = new SelectListItem(Toys[i], Toys[i], i == 0);
                SelectableToys.Add(item);
            }
        }

        private bool IsBadRequest(string[] answer)
        {
            if (answer.Length != 3)
            {
                return true;
            }

            foreach (var selectedToy in answer)
            {
                if (!Toys.Contains(selectedToy))
                {
                    return true;
                }
            }
            return false;
        }
    }
}