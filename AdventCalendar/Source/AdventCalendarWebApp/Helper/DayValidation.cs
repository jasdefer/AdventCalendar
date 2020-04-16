using AdventCalendarWebApp.Helper.TimeProvider;

namespace AdventCalendarWebApp.Helper
{
    public class DayValidation
    {
        private readonly ITimeProvider timeProvider;

        public DayValidation(ITimeProvider timeProvider)
        {
            this.timeProvider = timeProvider;
        }

        public bool HasAccess(int day)
        {
            var now = timeProvider.Now();
            return now >= Dates.Doors2020[day];
        }
    }
}