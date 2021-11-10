using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard;

public class Note04Model : PageModel
{
    private const int door = 4;
    private readonly DayValidation dayValidation;

    public Note04Model(DayValidation dayValidation)
    {
        this.dayValidation = dayValidation;
    }

    public IActionResult OnGet()
    {
        if (!dayValidation.HasAccess2020(door))
        {
            return RedirectToPage("Index", new { invalidDoor = door });
        }

        return Page();
    }
}
