using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard;

public class Note02Model : PageModel
{
    private readonly DayValidation dayValidation;
    private const int door = 2;

    public Note02Model(DayValidation dayValidation)
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
        if (string.Equals(answer, "ambulance", StringComparison.InvariantCultureIgnoreCase))
        {
            Solved = true;
        }
        else
        {
            ModelState.AddModelError(string.Empty, $"No, {answer} is not the Christmas wish from Mathilda.");
        }
        return Page();

    }

    public IActionResult OnPost()
    {
        return RedirectToPage("Note02", new { answer = Answer });
    }
}
