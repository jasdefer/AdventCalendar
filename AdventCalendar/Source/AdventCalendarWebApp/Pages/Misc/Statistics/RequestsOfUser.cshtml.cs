using AdventCalendarWebApp.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace AdventCalendarWebApp.Pages.Misc.Statistics;

public class RequestsOfUserModel : PageModel
{
    public record RequestsPerDay(DateTime Day, int NumberOfRequests);

    private readonly AzureHelper azureHelper;
    private readonly IConfiguration configuration;

    public RequestsOfUserModel(AzureHelper azureHelper,
        IConfiguration configuration)
    {
        this.azureHelper = azureHelper;
        this.configuration = configuration;
    }

    public IReadOnlyList<RequestsPerDay> Requests { get; set; }
    public string UserId { get; set; }

    public void OnGet(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            userId = HttpContext.GetOrCreateUserId();
        }
        UserId = userId;
        var table = azureHelper.GetTableReference(configuration["StorageData:RequestTableName"]);
        var query = new TableQuery<HttpRequestLog>()
            .Where(TableQuery.GenerateFilterCondition(nameof(HttpRequestLog.UserId), QueryComparisons.Equal, userId));
        var result = table.ExecuteQuery(query);
        var date = result.Min(x => x.RequestTimestamp).Date;
        var max = result.Min(x => x.RequestTimestamp).Date;
        var requests = new Dictionary<DateTime, int>();
        while (date <= max)
        {
            requests.Add(date, 0);
            date = date.AddDays(1);
        }
        Requests = result
            .GroupBy(x => x.RequestTimestamp.Date)
            .Select(x => new RequestsPerDay(x.Key, x.Count()))
            .OrderBy(x => x.Day)
            .ToArray();
    }
}
