using AdventCalendarWebApp.Helper;
using AdventCalendarWebApp.Model;
using AdventCalendarWebApp.Pages._2021;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendarWebApp.Pages.Misc.Statistics._2021
{
    public class WikiPagePicksModel : PageModel
    {
        public record PickData(string Pick, int NumberOfAttempts);

        private readonly AzureHelper azureHelper;
        private readonly IConfiguration configuration;

        public WikiPagePicksModel(AzureHelper azureHelper,
            IConfiguration configuration)
        {
            this.azureHelper = azureHelper;
            this.configuration = configuration;
        }

        public IReadOnlyList<PickData> Picks { get; set; }
        public TimeSpan AverageSolveDuration { get; set; }
        public int NumberOfCorrectSolutions { get; set; }
        public int Day { get; set; }

        public void OnGet(int day)
        {
            day = Math.Max(2, day);
            Day = day;
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
        }
    }
}