using AdventCalendarWebApp.Helper;
using AdventCalendarWebApp.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace AdventCalendarWebApp.Pages.Misc.Statistics;

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
        var query = new TableQuery<HttpRequestLog>()
            .OrderBy(nameof(HttpRequestLog.RequestTimestamp));
        var result = table.ExecuteQuery(query);
        var date = result.Min(x => x.RequestTimestamp).Date;
        var max = result.Max(x => x.RequestTimestamp).Date;
        var requests = new Dictionary<DateTime, int>();
        while (date <= max)
        {
            requests.Add(date, 0);
            date = date.AddDays(1);
        }
        Requests = requests
            .Select(x => new RequestsPerDay(x.Key, result.Where(y => y.RequestTimestamp.Date == x.Key).Count()))
            .OrderBy(x => x.Day)
            .ToArray();
    }
}
