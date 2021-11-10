using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard;

public class Note03Model : PageModel
{
    private readonly DayValidation dayValidation;
    private const int door = 3;
    private static readonly string[] Presents = new string[]
    {
            "Sock",
            "Car",
            "Ball",
            "Dice",
            "Book",
            "Sword",
            "Teddy"
    };


    [BindProperty]
    public string[] SelectedPresents { get; set; }
    public List<SelectListItem> SelectablePresents { get; private set; }

    public Note03Model(DayValidation dayValidation)
    {
        this.dayValidation = dayValidation;
    }

    public bool Solved { get; set; } = false;

    private void SetupSelectableList()
    {
        SelectablePresents = new List<SelectListItem>();
        for (int i = 0; i < Presents.Length; i++)
        {
            SelectablePresents.Add(new SelectListItem(Presents[i], Presents[i], false));
        }
    }

    private bool IsBadRequest(string[] selectedPresents)
    {
        if (selectedPresents.Length != 7)
        {
            return true;
        }

        foreach (var selectedPresent in selectedPresents)
        {
            if (!Presents.Contains(selectedPresent))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsCorrectSolution(string[] selectedPresents)
    {
        var bagOne = selectedPresents.Take(4).ToArray();
        var bagTwo = selectedPresents.Skip(4).Take(3).ToArray();
        if (bagOne.Contains(Presents[0]) &&
            bagOne.Contains(Presents[6]) &&
            bagOne.Contains(Presents[1]) &&
            bagOne.Contains(Presents[4]) &&
            bagTwo.Contains(Presents[2]) &&
            bagTwo.Contains(Presents[3]) &&
            bagTwo.Contains(Presents[5]))
        {
            return true;
        }
        return false;
    }

    public IActionResult OnGet(string[] answer)
    {
        if (!dayValidation.HasAccess2020(door))
        {
            return RedirectToPage("Index", new { invalidDoor = door });
        }
        SetupSelectableList();
        if (answer == null || answer.Length == 0)
        {
            return Page();
        }
        if (IsBadRequest(answer))
        {
            return BadRequest();
        }
        if (answer.Distinct().Count() != Presents.Length)
        {
            ModelState.AddModelError(string.Empty, "Each present must be in exactly one toy bag. It is not possible to store a single present in two toy bags.");
        }
        else if (IsCorrectSolution(answer))
        {
            Solved = true;
        }
        else
        {
            ModelState.AddModelError(string.Empty, $"Hm, that is a good answer, but I think there is even a better one.");
        }
        SelectedPresents = answer;
        return Page();
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("Note03", new { answer = SelectedPresents });
    }
}
