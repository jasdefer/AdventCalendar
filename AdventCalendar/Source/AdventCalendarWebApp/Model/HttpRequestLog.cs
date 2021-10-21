using Microsoft.Azure.Cosmos.Table;
using System;

namespace AdventCalendarWebApp.Model
{
    public class HttpRequestLog : TableEntity
    {
        public DateTime RequestTimestamp { get; set; }
        public string Url { get; set; }
        public string UserId { get; set; }
        public string Method { get; set; }
    }
}