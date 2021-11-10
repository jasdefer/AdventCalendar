using AdventCalendarWebApp.Helper.TimeProvider;
using AdventCalendarWebApp.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace AdventCalendarWebApp.Pages.Misc.Statistics;

public class NumberOfRequestsModel : PageModel
{
    public record UrlRequest(string BaseUrl, int NumberOfRequests);

    private readonly AzureHelper azureHelper;
    private readonly IConfiguration configuration;
    private readonly ITimeProvider timeProvider;

    public NumberOfRequestsModel(AzureHelper azureHelper,
        IConfiguration configuration,
        ITimeProvider timeProvider)
    {
        this.azureHelper = azureHelper;
        this.configuration = configuration;
        this.timeProvider = timeProvider;
    }

    public IReadOnlyList<UrlRequest> UrlRequests { get; set; }

    public void OnGet()
    {
        var table = azureHelper.GetTableReference(configuration["StorageData:RequestTableName"]);
        var today = timeProvider.Now().Date;
        var query = new TableQuery<HttpRequestLog>();
        if (today.Month == 12 && today.Day <= 24)
        {
            query = new TableQuery<HttpRequestLog>()
                .Where(TableQuery.GenerateFilterConditionForDate(nameof(HttpRequestLog.RequestTimestamp), QueryComparisons.LessThan, today));
        }
        var result = table.ExecuteQuery(query);
        UrlRequests = result
            .GroupBy(x => x.BaseUrl)
            .Select(x => new UrlRequest(x.Key, x.Count()))
            .OrderByDescending(x => x.NumberOfRequests)
            .ToArray();
    }
}
