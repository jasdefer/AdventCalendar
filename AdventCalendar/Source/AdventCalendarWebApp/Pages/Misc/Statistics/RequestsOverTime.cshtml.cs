using AdventCalendarWebApp.Helper;
using AdventCalendarWebApp.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendarWebApp.Pages.Misc.Statistics
{
    public class RequestsOverTimeModel : PageModel
    {
        public record RequestsPerDay(DateTime Day, int NumberOfRequests);

        private readonly AzureHelper azureHelper;
        private readonly IConfiguration configuration;

        public RequestsOverTimeModel(AzureHelper azureHelper,
            IConfiguration configuration)
        {
            this.azureHelper = azureHelper;
            this.configuration = configuration;
        }

        public IReadOnlyList<RequestsPerDay> Requests { get; set; }
        public void OnGet()
        {
            var table = azureHelper.GetTableReference(configuration["StorageData:RequestTableName"]);
            var query = new TableQuery<HttpRequestLog>();
            var result = table.ExecuteQuery(query);
            var date = result.Min(x => x.RequestTimestamp).Date;
            var max = result.Min(x => x.RequestTimestamp).Date;
            var requests = new Dictionary<DateTime, int>();
            while (date <= max)
            {
                requests.Add(date, 0);
                date = date.AddDays(1);
            }
            Requests = result.GroupBy(x => x.RequestTimestamp.Date).Select(y => new RequestsPerDay(y.Key, y.Count())).ToArray();
        }
    }
}