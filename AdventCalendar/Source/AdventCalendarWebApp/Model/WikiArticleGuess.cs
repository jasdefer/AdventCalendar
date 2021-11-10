using Microsoft.Azure.Cosmos.Table;

namespace AdventCalendarWebApp.Model;

public class WikiArticleGuess : TableEntity
{
    public DateTime GuessTimestamp { get; set; }
    public string UserId { get; set; }
    public string Guess { get; set; }
    public int Day { get; set; }
    public bool IsCorrect { get; set; }
    public int NumberOfHints { get; set; }
    public int NumberOfGuesses { get; set; }
    public double SolveDurationSeconds { get; set; }
}
