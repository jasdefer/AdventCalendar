using AdventCalendarWebApp.Helper;
using AdventCalendarWebApp.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace AdventCalendarWebApp.Pages.Misc.Statistics._2021;

public class OverviewModel : PageModel
{
    public record DayStat(int Day,
        int NumberOfIncorrectAttempts,
        int NumberOfCorrectAttempts,
        double AverageSolveSeconds);

    private readonly AzureHelper azureHelper;
    private readonly IConfiguration configuration;

    public IReadOnlyList<DayStat> DayStats { get; set; }

    public OverviewModel(AzureHelper azureHelper,
        IConfiguration configuration)
    {
        this.azureHelper = azureHelper;
        this.configuration = configuration;
    }

    public void OnGet()
    {
        var table = azureHelper.GetTableReference(configuration["StorageData:2021WikiArticleGuessesTableName"]);
        var articleGuessQuery = new TableQuery<WikiArticleGuess>();
        var articleGuessResult = table.ExecuteQuery(articleGuessQuery).ToArray();
        var dayStats = articleGuessResult
            .GroupBy(x => x.Day)
            .Select(x => new DayStat(x.Key,
                x.Where(y => y.IsCorrect == false).Count(),
                x.Where(y => y.IsCorrect).Count(),
                x.Where(y => y.IsCorrect).Count() < 1 ? 0d : x.Where(y => y.IsCorrect).Average(y => y.SolveDurationSeconds)))
            .ToList();
        table = azureHelper.GetTableReference(configuration["StorageData:2021WikiPagePicksTableName"]);
        var pagePickQuery = new TableQuery<WikiPagePick>();
        var pagePickResult = table.ExecuteQuery(pagePickQuery).ToArray();
        dayStats.AddRange(pagePickResult
            .GroupBy(x => x.Day)
            .Select(x => new DayStat(x.Key,
                x.Where(y => y.IsCorrect == false).Count(),
                x.Where(y => y.IsCorrect).Count(),
                x.Where(y => y.IsCorrect).Count() < 1 ? 0d : x.Where(y => y.IsCorrect).Average(y => y.SolveDurationSeconds))));
        DayStats = dayStats.OrderBy(x => x.Day).ToArray();
    }
}
