using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2020.Otis;

public class IndexModel : PageModel
{
    private readonly DayValidation dayValidation;
    private const int door = 20;
    public IndexModel(DayValidation dayValidation)
    {
        this.dayValidation = dayValidation;
    }
    public bool CanSolve { get; private set; }
    public bool IsSolved { get; private set; } = false;
    [BindProperty]
    public string Answer { get; set; }
    public IActionResult OnGet(string answer)
    {
        CanSolve = dayValidation.HasAccess2020(door);
        if (!CanSolve || string.IsNullOrEmpty(answer))
        {
            return Page();
        }

        if (CompareHelper.AreEqual(answer, "book"))
        {
            ModelState.AddModelError(string.Empty, $"Yeah, it could be a book. But which one, what is the missing book called?");
        }
        else if (CompareHelper.AreEqual(answer, "gilli") ||
            CompareHelper.AreEqual(answer, "seran"))
        {
            ModelState.AddModelError(string.Empty, $"No, the book {answer} is on the floor.");
        }
        else if (CompareHelper.AreEqual(answer, "sulien"))
        {
            ModelState.AddModelError(string.Empty, "I think Odin never owned the fifth book sulien.");
        }
        else if (CompareHelper.AreEqual(answer, "ring"))
        {
            ModelState.AddModelError(string.Empty, "Are you sure the ring is gone?");
        }
        else if (CompareHelper.AreEqual(answer, "mohidna"))
        {
            IsSolved = true;
        }
        else
        {
            ModelState.AddModelError(string.Empty, $"No, {answer} is not missing.");
        }
        Answer = answer;
        return Page();
    }

    public IActionResult OnPost(string answer)
    {
        return RedirectToPage("Index", new { answer = Answer });
    }
}
