using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.SnowCannon;

public class SwitchRoomModel : PageModel
{
    private static readonly bool[] Switches = new bool[]
    {
            true,
            false,
            true,
            true,
            true,
            true
    };

    public bool Switch { get; set; }
    public IActionResult OnGet(int? door)
    {
        if (!door.HasValue ||
            door.Value < 1 ||
            door.Value > Switches.Length + 1)
        {
            return NotFound();
        }
        Switch = Switches[door.Value - 1];
        return Page();
    }
}
