using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard;

public class Note21Model : PageModel
{
    private const int door = 21;
    private readonly DayValidation dayValidation;

    public Note21Model(DayValidation dayValidation)
    {
        this.dayValidation = dayValidation;
    }
    public bool Solved { get; set; }
    [BindProperty]
    public int Answer1 { get; set; }
    [BindProperty]
    public int Answer2 { get; set; }
    public IActionResult OnGet(int? answer1 = null, int? answer2 = null)
    {
        if (!dayValidation.HasAccess2020(door))
        {
            return RedirectToPage("Index", new { invalidDoor = door });
        }
        if (!answer1.HasValue ||
            !answer2.HasValue)
        {
            return Page();
        }
        if ((answer1 == 6 &&
            answer2 == 8) ||
            (answer1 == 8 &&
            answer2 == 6))
        {
            Solved = true;
        }
        else
        {
            ModelState.AddModelError(string.Empty, $"No, {answer1} and {answer2} is wrong.");
        }
        Answer1 = answer1.Value;
        Answer2 = answer2.Value;
        return Page();
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("Note21", new { answer1 = Answer1, answer2 = Answer2 });
    }
}
