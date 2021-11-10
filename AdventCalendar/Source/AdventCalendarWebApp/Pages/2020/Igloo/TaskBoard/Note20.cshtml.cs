using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard;

public class Note20Model : PageModel
{
    private readonly DayValidation dayValidation;
    private const int door = 20;

    public Note20Model(DayValidation dayValidation)
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
