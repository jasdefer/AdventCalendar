using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard;

public class Note17Model : PageModel
{
    private readonly DayValidation dayValidation;
    private const int door = 17;

    public Note17Model(DayValidation dayValidation)
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
