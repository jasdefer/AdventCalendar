using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard;

public class Note18Model : PageModel
{
    private readonly DayValidation dayValidation;
    private const int door = 18;
    private const int CorrectSolution = 18;
    private static readonly int[] weights = new int[] { 2, 1, 8, 3, 6, 7, 5, 9 };
    private static readonly int[] presents = new int[] { 3, 2, 8, 4, 6, 7, 5, 9 };

    public Note18Model(DayValidation dayValidation)
    {
        this.dayValidation = dayValidation;
    }

    public int MaxWeight { get; } = 16;
    public bool Solved { get; private set; }

    [BindProperty]
    public bool Answer1 { get; set; }
    [BindProperty]
    public bool Answer2 { get; set; }
    [BindProperty]
    public bool Answer3 { get; set; }
    [BindProperty]
    public bool Answer4 { get; set; }
    [BindProperty]
    public bool Answer5 { get; set; }
    [BindProperty]
    public bool Answer6 { get; set; }
    [BindProperty]
    public bool Answer7 { get; set; }
    [BindProperty]
    public bool Answer8 { get; set; }

    public IActionResult OnGet(bool[] answer = null)
    {
        if (!dayValidation.HasAccess2020(door))
        {
            return RedirectToPage("Index", new { invalidDoor = door });
        }
        if (answer == null || answer.Length == 0)
        {
            return Page();
        }
        if (answer.Length != weights.Length)
        {
            return BadRequest();
        }
        var totalWeight = 0;
        var numberOfPresents = 0;
        for (int i = 0; i < answer.Length; i++)
        {
            if (answer[i])
            {
                totalWeight += weights[i];
                numberOfPresents += presents[i];
            }
        }
        if (totalWeight > MaxWeight)
        {
            ModelState.AddModelError(string.Empty, $"The gift bags you selected weigh {totalWeight}kg in total and exceed the limit of {MaxWeight}");
        }
        else if (numberOfPresents >= CorrectSolution)
        {
            Solved = true;
        }
        else
        {
            ModelState.AddModelError(string.Empty, $"You select gift bags with a total number of {numberOfPresents} presents. I think there is a better solution.");
        }
        Answer1 = answer[0];
        Answer2 = answer[1];
        Answer3 = answer[2];
        Answer4 = answer[3];
        Answer5 = answer[4];
        Answer6 = answer[5];
        Answer7 = answer[6];
        Answer8 = answer[7];
        return Page();
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("Note18", new { answer = new bool[] { Answer1, Answer2, Answer3, Answer4, Answer5, Answer6, Answer7, Answer8 } });
    }
}
