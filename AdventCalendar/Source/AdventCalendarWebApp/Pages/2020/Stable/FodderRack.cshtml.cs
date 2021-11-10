using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendarWebApp.Pages._2020.Stable;

public class FodderRackModel : PageModel
{
    private readonly DayValidation dayValidation;
    private const int day = 4;
    private static readonly string[] Meals = new string[]
    {
            "Mushroom",
            "Grass",
            "Leaf",
            "Acorn"
    };

    public FodderRackModel(DayValidation dayValidation)
    {
        this.dayValidation = dayValidation;
        CanSolve = dayValidation.HasAccess2020(day);
    }

    public bool CanSolve { get; }
    public bool IsSolved { get; private set; } = false;
    public List<SelectListItem> SelectableMeals { get; private set; }
    [BindProperty]
    public string[] SelectedMeals { get; set; }

    public IActionResult OnGet(string[] answer)
    {
        if (!CanSolve)
        {
            return Page();
        }

        SetupSelectableMeals();
        if (answer == null || answer.Length == 0)
        {
            return Page();
        }
        var isBadRequest = IsBadRequest(answer);
        if (isBadRequest)
        {
            return BadRequest();
        }
        var isCorrectSolution = IsCorrectSolution(answer);
        if (isCorrectSolution)
        {
            IsSolved = true;
        }
        else
        {
            var isMisleadAnswer = IsMisleadAnswer(answer);
            if (isMisleadAnswer)
            {
                ModelState.AddModelError(string.Empty, "Strange, the food-reindeer-assignment exactly follows the instruction on the note. But the reindeer do not eat their food. Someone mixed up the order of the troughs.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "The reindeers don't eat their food. This seems to be the wrong assignment.");
            }
        }
        SelectedMeals = answer;
        return Page();
    }

    private bool IsCorrectSolution(string[] answer)
    {
        return answer[0] == Meals[1] &&
            answer[1] == Meals[3] &&
            answer[2] == Meals[2] &&
            answer[3] == Meals[0];
    }

    private bool IsMisleadAnswer(string[] answer)
    {
        return answer[0] == Meals[3] &&
            answer[1] == Meals[0] &&
            answer[2] == Meals[1] &&
            answer[3] == Meals[2];
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("FodderRack", new { answer = SelectedMeals });
    }

    private bool IsBadRequest(string[] selectedMeals)
    {
        if (selectedMeals.Length != Meals.Length)
        {
            return true;
        }

        foreach (var selectedMeal in selectedMeals)
        {
            if (!Meals.Contains(selectedMeal))
            {
                return true;
            }
        }
        return false;
    }

    private void SetupSelectableMeals()
    {
        SelectableMeals = new List<SelectListItem>();
        for (int i = 0; i < Meals.Length; i++)
        {
            var item = new SelectListItem(Meals[i], Meals[i], i == 0);
            SelectableMeals.Add(item);
        }
    }
}
