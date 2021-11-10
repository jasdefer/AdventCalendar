using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard;

public class Note06Model : PageModel
{
    private readonly DayValidation dayValidation;
    private const int door = 6;

    public Note06Model(DayValidation dayValidation)
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
        if (CompareHelper.AreEqual("inception", answer))
        {
            Solved = true;
        }
        else
        {
            ModelState.AddModelError(string.Empty, $"The path connection {answer} does not match the available instructions.");
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("Note06", new { answer = Answer });
    }
}
