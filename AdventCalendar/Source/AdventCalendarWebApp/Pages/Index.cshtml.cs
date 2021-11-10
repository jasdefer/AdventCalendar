using AdventCalendarWebApp.Helper.TimeProvider;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages;

public class IndexModel : PageModel
{
    private static readonly int[] DaysOrdered = Enumerable.Range(1, 24).ToArray();
    private readonly ITimeProvider timeProvider;

    public int[] Days { get; private set; }
    public int Day { get; set; }

    public IndexModel(ITimeProvider timeProvider)
    {
        this.timeProvider = timeProvider;
        var today = timeProvider.Now().Date;
        if (today >= Dates.Doors2021[24])
        {
            Day = 24;
        }
        else if (today < Dates.Doors2021[1])
        {
            Day = 0;
        }
        else
        {
            Day = today.Day;
        }
    }
    public void OnGet()
    {
        var random = new Random();
        Days = DaysOrdered
            .Take(Day)
            .OrderBy(x => random.NextDouble()).ToArray();
    }
}
