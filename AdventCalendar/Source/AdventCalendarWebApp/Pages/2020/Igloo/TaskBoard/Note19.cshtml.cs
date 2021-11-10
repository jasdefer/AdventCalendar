using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard;

public class Note19Model : PageModel
{
    private readonly DayValidation dayValidation;
    private const int door = 19;

    public Note19Model(DayValidation dayValidation)
    {
        this.dayValidation = dayValidation;
    }
    public bool Solved { get; set; }
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
        else if (CompareHelper.AreEqual(answer, "library"))
        {
            Solved = true;
        }
        else
        {
            ModelState.AddModelError(string.Empty, $"No, {answer} is not the correct solution.");
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("Note19", new { answer = Answer });
    }
}
