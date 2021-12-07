using AdventCalendarWebApp.Model;
using AdventCalendarWebApp.Pages._2021;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace AdventCalendarWebApp.Pages.Misc.Statistics._2021;

public class MyResultsModel : PageModel
{
    public record DayStat(int Day, string Answer, bool IsCorrect, TimeSpan SolveDuration);
    private readonly AzureHelper azureHelper;
    private readonly IConfiguration configuration;

    public IReadOnlyList<DayStat> DayStats { get; set; }

    public MyResultsModel(AzureHelper azureHelper,
        IConfiguration configuration)
    {
        this.azureHelper = azureHelper;
        this.configuration = configuration;
    }

    public void OnGet()
    {
        var dayStatList = new List<DayStat>();
        var table = azureHelper.GetTableReference(configuration["StorageData:2021WikiArticleGuessesTableName"]);
        var userId = HttpContext.GetOrCreateUserId();
        var articleGuessQuery = new TableQuery<WikiArticleGuess>()
            .Where($"{nameof(WikiArticleGuess.UserId)} eq '{userId}'");
        var articleGuessResult = table.ExecuteQuery(articleGuessQuery).ToArray();
        dayStatList = articleGuessResult
            .Select(x => new DayStat(x.Day,
            x.Guess,
            x.IsCorrect,
            TimeSpan.FromSeconds(x.SolveDurationSeconds)))
            .ToList();
        var articlePickQuery = new TableQuery<WikiPagePick>()
            .Where($"{nameof(WikiPagePick.UserId)} eq '{userId}'");
        table = azureHelper.GetTableReference(configuration["StorageData:2021WikiPagePicksTableName"]);
        var articlePickResult = table.ExecuteQuery(articlePickQuery).ToArray();
        dayStatList.AddRange(articlePickResult
                .Select(x => new DayStat(x.Day,
                    WikiPagePickerModel.OptionStrings[x.Day / 2 - 1][x.Pick],
                    x.IsCorrect,
                    TimeSpan.FromSeconds(x.SolveDurationSeconds))));
        DayStats = dayStatList
            .OrderBy(x => x.Day)
            .ToArray();
    }
}
