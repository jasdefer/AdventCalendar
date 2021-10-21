using Microsoft.Azure.Cosmos.Table;
using System;

namespace AdventCalendarWebApp.Model
{
    public class WikiPagePick : TableEntity
    {
        public string UserId { get; set; }
        public int Pick { get; set; }
        public int Day { get; set; }
        public bool IsCorrect { get; set; }
        public TimeSpan SolveDuration { get; set; }
        public int NumberOfGuesses { get; set; }
    }
}