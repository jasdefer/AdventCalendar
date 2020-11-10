using AdventCalendarWebApp.Helper.TimeProvider;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.EastWing
{
    public class IndexModel : PageModel
    {
        private readonly ITimeProvider timeProvider;

        public bool DisplayHolidayLights { get; set; }
        public bool SolvedTheRiddle { get; set; }

        public IndexModel(ITimeProvider timeProvider)
        {
            this.timeProvider = timeProvider;
        }

        public void OnGet(bool solvedTheRiddle = false)
        {
            SolvedTheRiddle = solvedTheRiddle;
            var now = timeProvider.Now();
            if (SolvedTheRiddle ||
                now >= Dates.Doors2020[2])
            {
                DisplayHolidayLights = true;
            }
        }
    }
}