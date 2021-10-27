using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard
{
    public class Note08Model : PageModel
    {
        private readonly DayValidation dayValidation;
        private const int door = 8;

        //Column and row labels: S,A,B,C,D,E,F,G,H
        private readonly int[,] distanceMatrix = new int[9, 9]
        {
            { 0,5 ,5 ,3,10,9,3,6,7},
            { 5,0 ,10,4,7 ,6,2,5,8},
            { 5,10,0 ,6,7 ,6,8,5,4},
            { 3,4 ,6 ,0,7 ,6,2,3,4},
            {10,7 ,7 ,7,0 ,3,9,4,3},
            { 9,6 ,6 ,6,3 ,0,8,3,4},
            { 3,2 ,8 ,2,9 ,8,0,5,6},
            { 6,5 ,5 ,3,4 ,3,5,0,3},
            { 7,8, 4, 4,3 ,4,6,3,0 }
        };

        public Note08Model(DayValidation dayValidation)
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
            if (answer.Length != 10 ||
                char.ToLowerInvariant(answer[0]) != 's' ||
                char.ToLowerInvariant(answer[^1]) != 's')
            {
                ModelState.AddModelError(string.Empty, $"You answer should contain exactly 10 letters. It should start and end with S and contain every letter from A to H.");
                return Page();
            }

            var distance = 0;
            var lastIndex = 0;
            var households = Enumerable.Range(1, 8).ToHashSet();
            for (int i = 1; i < answer.Length - 1; i++)
            {
                //Each character of the answer can be converted to a number
                //A  = 65, B = 66 etc.
                //A has the index 1 in the distanceMatrix array, B has the index 2, etc.
                var index = char.ToUpperInvariant(answer[i]) - 64;
                if (index < 1 ||
                    index > 8)
                {
                    //Only A to H which are the indices 1 to 8 are valid inputs
                    if (index == 19)
                    {
                        //Add an extra error message if the input contains a S inside the answer
                        ModelState.AddModelError(string.Empty, $"Santa should visit S only at the start and end of his journey. Not in between household visits.");
                        return Page();
                    }
                    ModelState.AddModelError(string.Empty, $"There is no household {answer[i]}");
                    return Page();
                }

                if (!households.Contains(index))
                {
                    ModelState.AddModelError(string.Empty, $"Every household should be visited exactly once. With your current solution, Santa needs to visit {answer[i]} at least twice.");
                    return Page();
                }
                households.Remove(index);
                distance += distanceMatrix[lastIndex, index];
                lastIndex = index;
            }
            //Add the distance from the last household to S
            distance += distanceMatrix[lastIndex, 0];

            if (distance > 30)
            {
                ModelState.AddModelError(string.Empty, $"If I counted correctly, your answer has a total length of {distance} steps. I think there is a shorter connection.");
            }
            else
            {
                Solved = true;
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("Note08", new { answer = Answer });
        }
    }
}