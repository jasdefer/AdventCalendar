using AdventCalendarWebApp.Helper.TimeProvider;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;

namespace AdventCalendarWebApp.Pages.Helper;

public class SetCustomTimeModel : PageModel
{
    public SetCustomTimeModel(ITimeProvider timeProvider, IWebHostEnvironment env)
    {
        TimeProvider = timeProvider;
        timeIsAdjustable = env.IsDevelopment();
    }

    public int? Index { get; set; }
    public ITimeProvider TimeProvider { get; set; }

    private readonly bool timeIsAdjustable;

    public IActionResult OnGet(int? index = null)
    {
        if (!timeIsAdjustable ||
            !(TimeProvider is DebugTimeProvider))
        {
            return NotFound();
        }
        if (!index.HasValue)
        {
            return Page();
        }
        if (index < 0 || index > 25)
        {
            index = 0;
        }
        DebugTimeProvider.DoorIndex = index.Value;
        Index = index;
        return Page();
    }
}
