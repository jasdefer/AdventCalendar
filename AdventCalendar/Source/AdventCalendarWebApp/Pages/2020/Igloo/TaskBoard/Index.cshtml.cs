using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard;

public class IndexModel : PageModel
{
    public int? InvalidDoor { get; set; }

    public void OnGet(int? invalidDoor)
    {
        InvalidDoor = invalidDoor;
    }
}
