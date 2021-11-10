using AdventCalendarWebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace AdventCalendarWebApp.Pages.Misc.Statistics._2021;

public class WikiArticleGuessesModel : PageModel
{
    public record GuessData(string Guess, int NumberOfGuesses);

    private readonly AzureHelper azureHelper;
    private readonly IConfiguration configuration;
    private readonly DayValidation dayValidation;

    public WikiArticleGuessesModel(AzureHelper azureHelper,
        IConfiguration configuration,
        DayValidation dayValidation)
    {
        this.azureHelper = azureHelper;
        this.configuration = configuration;
        this.dayValidation = dayValidation;
    }

    public IReadOnlyList<GuessData> Guesses { get; set; }
    public TimeSpan AverageSolveDuration { get; set; }
    public int NumberOfCorrectSolutions { get; set; }
    public int Day { get; set; }

    public IActionResult OnGet(int day)
    {
        day = Math.Max(1, day);
        Day = day;
        if (!dayValidation.HasAccess2021(day + 1))
        {
            return NotFound();
        }
        var table = azureHelper.GetTableReference(configuration["StorageData:2021WikiArticleGuessesTableName"]);
        var query = new TableQuery<WikiArticleGuess>()
            .Where($"{nameof(WikiArticleGuess.Day)} eq {day}");
        var result = table.ExecuteQuery(query).ToArray();
        Guesses = result.GroupBy(x => x.Guess).Select(y => new GuessData(y.Key, y.Count())).ToArray();
        var durations = result.Where(x => x.IsCorrect &&
        x.SolveDurationSeconds >= 2 &&
        x.SolveDurationSeconds <= 3 * 60 * 60)
            .Select(x => x.SolveDurationSeconds)
            .ToArray();
        if (durations.Length > 0)
        {
            AverageSolveDuration = TimeSpan.FromSeconds(Math.Round(durations.Average()));
        }
        NumberOfCorrectSolutions = result.Where(x => x.IsCorrect).GroupBy(x => x.UserId).Count();
        return Page();
    }
}
