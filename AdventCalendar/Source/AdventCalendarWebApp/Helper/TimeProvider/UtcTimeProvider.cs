namespace AdventCalendarWebApp.Helper.TimeProvider;

public class UtcTimeProvider : ITimeProvider
{
    public DateTime Now()
    {
        return DateTime.UtcNow;
    }
}
