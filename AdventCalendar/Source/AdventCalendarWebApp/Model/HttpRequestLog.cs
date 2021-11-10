using Microsoft.Azure.Cosmos.Table;

namespace AdventCalendarWebApp.Model;

public class HttpRequestLog : TableEntity
{
    public DateTime RequestTimestamp { get; set; }
    public string BaseUrl { get; set; }
    public string Arguments { get; set; }
    public string UserId { get; set; }
    public string Method { get; set; }
}
