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

        public bool HasAccess2020(int day)
        {
            var now = timeProvider.Now();
            return now >= Dates.Doors2020[day];
        }

        public bool HasAccess2021(int day)
        {
            var now = timeProvider.Now();
            if (day >= Dates.Doors2021.Length || day < 0)
            {
                return false;
            }
            return now >= Dates.Doors2021[day];
        }
    }
}