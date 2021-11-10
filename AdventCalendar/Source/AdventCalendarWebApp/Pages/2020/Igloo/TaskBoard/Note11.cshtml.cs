using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard;

public class Note11Model : PageModel
{
    private readonly DayValidation dayValidation;
    private const int door = 11;
    private static readonly string[] shapeNames = new string[]
    {
            "rhombus",
            "hexagon",
            "pentagon",
            "rectangle"
    };

    public Note11Model(DayValidation dayValidation)
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
        if (CompareHelper.AreEqual("moat", answer))
        {
            Solved = true;
        }
        else if (CompareHelper.Contains(shapeNames, answer))
        {
            ModelState.AddModelError(string.Empty, $"Repeating the names is not the answer. We need to combine the shape and the number.");
        }
        else
        {
            ModelState.AddModelError(string.Empty, $"No, {answer} cannot be the solution.");
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("Note11", new { answer = Answer });
    }
}
