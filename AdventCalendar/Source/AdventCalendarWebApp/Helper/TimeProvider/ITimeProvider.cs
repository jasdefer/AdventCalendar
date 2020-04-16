using System;

namespace AdventCalendarWebApp.Helper.TimeProvider
{
    public interface ITimeProvider
    {
        DateTime Now();
    }
}