using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendarWebApp.Pages._2020.Hideout
{
    public class MazeModel : PageModel
    {
        private readonly DayValidation dayValidation;
        private const int door = 17;
        private static readonly int[] CorrectPath = new int[] { 0, 1, -1, -1, 0, 0, -1, 1 };
        public int[] Path { get; set; } = System.Array.Empty<int>();
        public bool? Solved { get; private set; } = null;

        public MazeModel(DayValidation dayValidation)
        {
            this.dayValidation = dayValidation;
        }

        public IActionResult OnGet(List<int> path = null,
            int? direction = null,
            bool back = false)
        {
            if (!dayValidation.HasAccess(door))
            {
                return RedirectToPage("Index", new { rejected = true });
            }
            if (path == null)
            {
                path = new List<int>();
            }

            //Store the next step for the path
            if (direction.HasValue)
            {
                if (direction.Value < -1 ||
                    direction.Value > 1)
                {
                    return BadRequest();
                }
                path.Add(direction.Value);
            }
            if (path.Count == 0)
            {
                //The user is at the start of the maze
                return Page();
            }
            if (path.Any(x => x < -1 || x > 1) ||
                path.Count > CorrectPath.Length)
            {
                return BadRequest();
            }
            if (back)
            {
                //Go back one door
                path.RemoveAt(path.Count - 1);
                Path = path.ToArray();
                return Page();
            }
            if (path.Count == CorrectPath.Length)
            {
                Solved = IsCorrectPath(path);
            }
            Path = path.ToArray();
            return Page();
        }

        private static bool IsCorrectPath(List<int> path)
        {
            if (path.Count != CorrectPath.Length)
            {
                return false;
            }
            for (int i = 0; i < path.Count; i++)
            {
                if (path[i] != CorrectPath[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}