using AdventCalendarWebApp.Helper.TimeProvider;
using AdventCalendarWebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace AdventCalendarWebApp.Pages._2021;

public class WikiPagePickerModel : PageModel
{
    private readonly DayValidation dayValidation;
    private readonly AzureHelper azureHelper;
    private readonly IConfiguration configuration;
    private readonly ITimeProvider timeProvider;
    public static readonly IReadOnlyList<IReadOnlyList<string>> OptionStrings = new string[12][]
    {
            new string[]{ "Bill Gates","Warren Buffet","Mark Zuckerberg", "Batman", "Tintin" },
            new string[]{ "Tour de France", "Neil Armstrong", "Savanna","Helium","Jazz" },
            new string[]{ "Alcohol","Microprocessor","Walmart","Papyrus","DNA" },
            new string[]{ "Broccoli", "Kim Kardashian", "Area 51","Road","Mirror" },
            new string[]{ "Michelangelo", "Vincent van Gogh","Claude Monet", "Leonardo da Vinci", "Rembrandt","Pablo Picasso" },
            new string[]{ "Zebra", "Kim Kardashian", "Airbus A400M Atlas", "Andes","Hexagon","Whaam!" },
            new string[]{ "Same-sex marriage in Spain", "Bob Marley", "Chocolate", "Joe Jackson","Welding" },
            new string[]{ "Mathematics", "Bullfighting", "FC Bayern Munich", "Jousting", "Freight transport" },
            new string[]{ "Apple", "Liquefied natural gas", "Easel", "Cargo", "Headquarters" },
            new string[]{ "Algebra", "Military", "Ethics", "Crime", "Employment" },
            new string[]{ "Hypercube", "Brexit","Dancing", "Dementia with Lewy bodies", "Flag of Belarus" },
            new string[]{ "Bank", "Depression (mood)", "Watergate scandal", "Journalism", "Computer security" },
    };

    public static readonly int[] CorrectOptions = new int[12]
    {
            3,1,4,0,1,0,4,2,3,0,4,2
    };

    public static IReadOnlyList<IReadOnlyList<string>> Words = new string[12][]
    {
            new string[]{ "Bob Kane", "philanthropist", "parents","sidekick","justice","publication"},
            new string[]{ "Korean War","second group","dangerous","broadcast","victory" },
            new string[]{ "monomeric","sequence","specify","information","development","store","transcribed"},
            new string[]{ "branching","stalk","glucosinolate","resembles","family","classified","green" },
            new string[]{ "posthumously", "impressionist","decade","artworks","oil","landscapes","brushwork","depression","unsuccessful" },
            new string[]{ "asses","family","flies","recognisable","protected","distinctive","extant","inhabit","mountain"},
            new string[]{ "joints","laser","hazardous","automatic","molten","electrical","distinct","manual","parent"},
            new string[]{ "team", "tier","successful","sextuple","red","titles"},
            new string[]{ "conveyed","covers","perishable","final","facility","designed","handling"},
            new string[]{ "broad", "form", "study", "thread", "groups", "medicine", "letters", "essential", "major", "applying", "values"},
            new string[]{ "bisected","officials","referendum","occupation","historical","independence","hoist","introduced","staff" },
            new string[]{ "major", "time", "investigations", "grant ", "activities", "removed", "burglars", "articles", "resignation" }
    };

    public List<SelectListItem> Options { set; get; }
    public int Index => Day / 2 - 1;
    public int Day { get; set; }
    public ValidationState ValidationState { get; private set; } = ValidationState.NotValidated;

    public int? Answer { get; set; }
    public int NumberOfGuesses { get; set; }
    public DateTime StartOfGuessing { get; set; }
    public TimeSpan SolveDuration => timeProvider.Now() - StartOfGuessing;

    public WikiPagePickerModel(DayValidation dayValidation,
        AzureHelper azureHelper,
        IConfiguration configuration,
        ITimeProvider timeProvider)
    {
        this.dayValidation = dayValidation;
        this.azureHelper = azureHelper;
        this.configuration = configuration;
        this.timeProvider = timeProvider;
    }

    public async Task<IActionResult> OnGet(int day,
        int? answer,
        int numberOfGuesses = 0,
        DateTime? startOfGuessing = null)
    {
        var hasAccess = dayValidation.HasAccess2021(day);
        if (day % 2 == 1 || !hasAccess)
        {
            return NotFound();
        }
        Day = day;
        StartOfGuessing = startOfGuessing ?? timeProvider.Now();
        SetupOptions(Index);
        if (!answer.HasValue)
        {
            return Page();
        }
        NumberOfGuesses = ++numberOfGuesses;
        Answer = answer;
        ValidationState = answer.Value == CorrectOptions[Index] ? ValidationState.Correct : ValidationState.Incorrect;
        await LogWikiPagePick(answer.Value);

        return Page();
    }

    private async Task LogWikiPagePick(int answer)
    {
        var userId = HttpContext.GetOrCreateUserId();
        var wikiPagePick = new WikiPagePick()
        {
            PartitionKey = userId,
            RowKey = Guid.NewGuid().ToString(),
            UserId = userId,
            Day = Day,
            Pick = answer,
            SolveDurationSeconds = SolveDuration.TotalSeconds,
            NumberOfGuesses = NumberOfGuesses,
            IsCorrect = ValidationState == ValidationState.Correct
        };
        await azureHelper.AddObjectAsync(configuration["StorageData:2021WikiPagePicksTableName"], wikiPagePick);
    }

    private void SetupOptions(int index)
    {
        Options = new List<SelectListItem>();
        for (int i = 0; i < OptionStrings[index].Count; i++)
        {
            Options.Add(new SelectListItem(OptionStrings[index][i], ""));
        }
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("WikiPagePicker", new
        {
            day = Day,
            numberOfGuesses = NumberOfGuesses,
            answer = Answer,
            startOfGuessing = StartOfGuessing,
        });
    }
}
