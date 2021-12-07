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
        double AverageSolveSeconds,
        double MedianSolveSeconds,
        double AverageNumberOfGuesses,
        double MedianNumberOfGuesses);

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
            .Select(x =>
            {
                var correctGuesses = x
                    .Where(y => y.IsCorrect)
                    .OrderByDescending(y => y.SolveDurationSeconds)
                    .ToArray();
                var medianIndex = correctGuesses.Length / 2;
                var dayStat = new DayStat(x.Key,
                    x.Where(y => y.IsCorrect == false).Count(),
                    correctGuesses.Length,
                    correctGuesses.Length < 1 ? 0d : correctGuesses.Average(y => y.SolveDurationSeconds),
                    correctGuesses.Length < 1 ? 0d : correctGuesses[medianIndex].SolveDurationSeconds,
                    correctGuesses.Length < 1 ? 0d : correctGuesses.Average(y => y.NumberOfGuesses),
                    correctGuesses.Length < 1 ? 0d : correctGuesses.OrderBy(y => y.NumberOfGuesses).ElementAt(medianIndex).NumberOfGuesses);
                return dayStat;
            })
            .ToList();
        table = azureHelper.GetTableReference(configuration["StorageData:2021WikiPagePicksTableName"]);
        var pagePickQuery = new TableQuery<WikiPagePick>();
        var pagePickResult = table.ExecuteQuery(pagePickQuery).ToArray();
        dayStats.AddRange(pagePickResult
            .GroupBy(x => x.Day)
            .Select(x =>
            { 
                var correctPicks = x
                    .Where(y => y.IsCorrect)
                    .OrderByDescending(y => y.SolveDurationSeconds)
                    .ToArray();
                var medianIndex = correctPicks.Length / 2;
                var dayStat = new DayStat(x.Key,
                    x.Where(y => y.IsCorrect == false).Count(),
                    correctPicks.Length,
                    correctPicks.Length < 1 ? 0d : correctPicks.Average(y => y.SolveDurationSeconds),
                    correctPicks.Length < 1 ? 0d : correctPicks[medianIndex].SolveDurationSeconds,
                    correctPicks.Length < 1 ? 0d : correctPicks.Average(y => y.NumberOfGuesses),
                    correctPicks.Length < 1 ? 0d : correctPicks.OrderBy(y => y.NumberOfGuesses).ElementAt(medianIndex).NumberOfGuesses);
                return dayStat;
            }));
        DayStats = dayStats.OrderBy(x => x.Day).ToArray();
    }
}
