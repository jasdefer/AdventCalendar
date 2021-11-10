using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard;

public class Note24Model : PageModel
{
    private readonly DayValidation dayValidation;
    private const int door = 24;

    public Note24Model(DayValidation dayValidation)
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
        if (answer.Length <= 3)
        {
            ModelState.AddModelError(string.Empty, $"{answer} is not a good gift.");
        }
        else
        {
            Solved = true;
        }
        Answer = answer;
        return Page();

    }

    public IActionResult OnPost()
    {
        return RedirectToPage("Note24", new { answer = Answer });
    }
}
