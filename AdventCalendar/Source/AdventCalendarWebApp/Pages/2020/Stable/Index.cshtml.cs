using AdventCalendarWebApp.Helper.TimeProvider;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Stable;

public class Index : PageModel
{
    public Index(ITimeProvider timeProvider)
    {
        var hour = timeProvider.Now().Hour;
        IsDay = hour >= 8 && hour <= 18;
    }
    public bool IsDay { get; }
}
