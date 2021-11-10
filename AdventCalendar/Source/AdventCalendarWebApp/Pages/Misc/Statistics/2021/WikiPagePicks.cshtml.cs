using AdventCalendarWebApp.Helper;
using AdventCalendarWebApp.Model;
using AdventCalendarWebApp.Pages._2021;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace AdventCalendarWebApp.Pages.Misc.Statistics._2021;

public class WikiPagePicksModel : PageModel
{
    public record PickData(string Pick, int NumberOfAttempts);

    private readonly AzureHelper azureHelper;
    private readonly IConfiguration configuration;
    private readonly DayValidation dayValidation;

    public WikiPagePicksModel(AzureHelper azureHelper,
        IConfiguration configuration,
        DayValidation dayValidation)
    {
        this.azureHelper = azureHelper;
        this.configuration = configuration;
        this.dayValidation = dayValidation;
    }

    public IReadOnlyList<PickData> Picks { get; set; }
    public TimeSpan AverageSolveDuration { get; set; }
    public int NumberOfCorrectSolutions { get; set; }
    public int Day { get; set; }

    public IActionResult OnGet(int day)
    {
        day = Math.Max(2, day);
        Day = day;
        if (!dayValidation.HasAccess2021(day + 1))
        {
            return NotFound();
        }
        var table = azureHelper.GetTableReference(configuration["StorageData:2021WikiPagePicksTableName"]);
        var query = new TableQuery<WikiPagePick>()
            .Where($"{nameof(WikiPagePick.Day)} eq {day}");
        var result = table.ExecuteQuery(query).ToArray();
        Picks = result
            .GroupBy(x => x.Pick)
            .Select(y => new PickData(WikiPagePickerModel.OptionStrings[day / 2 - 1][y.Key], y.Count()))
            .ToArray();
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
