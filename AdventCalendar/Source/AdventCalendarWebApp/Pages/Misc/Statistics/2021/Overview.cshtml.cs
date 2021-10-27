using AdventCalendarWebApp.Helper;
using AdventCalendarWebApp.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendarWebApp.Pages.Misc.Statistics._2021
{
    public class OverviewModel : PageModel
    {
        public record DayStat(int Day,
            int NumberOfIncorrectAttempts,
            int NumberOfCorrectAttempts,
            double AverageSolveSeconds);

        private readonly AzureHelper azureHelper;

        public IReadOnlyList<DayStat> DayStats { get; set; }

        public OverviewModel(AzureHelper azureHelper)
        {
            this.azureHelper = azureHelper;
        }

        public void OnGet()
        {
            var table = azureHelper.GetTableReference("WikiArticleGuesses");
            var articleGuessQuery = new TableQuery<WikiArticleGuess>();
            var articleGuessResult = table.ExecuteQuery(articleGuessQuery).ToArray();
            var dayStats = articleGuessResult
                .GroupBy(x => x.Day)
                .Select(x => new DayStat(x.Key,
                    x.Where(y => y.IsCorrect == false).Count(),
                    x.Where(y => y.IsCorrect).Count(),
                    x.Where(y => y.IsCorrect).Count() < 1 ? 0d : x.Where(y => y.IsCorrect).Average(y => y.SolveDurationSeconds)))
                .ToList();
            table = azureHelper.GetTableReference("WikiPagePicks");
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
}