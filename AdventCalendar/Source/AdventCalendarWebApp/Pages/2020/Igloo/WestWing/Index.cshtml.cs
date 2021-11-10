using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AdventCalendarWebApp.Pages._2020.Igloo.WestWing;

public class IndexModel : PageModel
{
    private static readonly string[] correctSolution = new string[] { "d", "c", "e", "a", "b" };
    private readonly DayValidation dayValidation;

    [BindProperty]
    public bool HasMagnifier { get; set; }
    public bool CanSolve { get; set; }
    public bool Solved { get; set; }
    public List<SelectListItem> SelectableWraps { get; private set; }

    [BindProperty]
    public string[] SelectedWraps { get; set; }

    public IndexModel(DayValidation dayValidation)
    {
        this.dayValidation = dayValidation;
    }

    public IActionResult OnGet(bool hasMagnifier = false, string[] answer = null)
    {
        CanSolve = dayValidation.HasAccess2020(14);
        HasMagnifier = hasMagnifier & CanSolve;
        if (!CanSolve)
        {
            return Page();
        }
        SetupSelectableWraps();
        if (answer == null || answer.Length == 0)
        {
            return Page();
        }
        var isBadRequest = IsBadRequest(answer);
        if (isBadRequest)
        {
            return BadRequest();
        }
        var isCorrectAnswer = IsCorrectAnswer(answer);
        Solved = isCorrectAnswer;
        if (!isCorrectAnswer)
        {
            ModelState.AddModelError(string.Empty, $"Your answer ({string.Join(", ", answer)}) is false.");
        }
        SelectedWraps = answer;

        return Page();
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("Index", new { answer = SelectedWraps, hasMagnifier = HasMagnifier });
    }

    private static bool IsCorrectAnswer(string[] answer)
    {
        for (int i = 0; i < correctSolution.Length; i++)
        {
            var isCorrect = CompareHelper.AreEqual(answer[i], correctSolution[i]);
            if (!isCorrect)
            {
                return false;
            }
        }
        return true;
    }

    private void SetupSelectableWraps()
    {
        SelectableWraps = new List<SelectListItem>();
        var wraps = new string[] { "A", "B", "C", "D", "E" };
        for (int i = 0; i < wraps.Length; i++)
        {
            var item = new SelectListItem(wraps[i], wraps[i], i == 0);
            SelectableWraps.Add(item);
        }
    }

    private bool IsBadRequest(string[] answer)
    {
        if (answer.Length != 5)
        {
            return true;
        }

        foreach (var selectedWrap in answer)
        {
            if (!CompareHelper.Contains(correctSolution, selectedWrap))
            {
                return true;
            }
        }
        return false;
    }
}
