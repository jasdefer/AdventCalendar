using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace AdventCalendarWebApp.Pages._2020.Igloo.TaskBoard;

public class Note15Model : PageModel
{
    private readonly DayValidation dayValidation;
    private const int door = 15;

    public Note15Model(DayValidation dayValidation)
    {
        this.dayValidation = dayValidation;
    }

    public bool Solved { get; set; } = false;

    [BindProperty]
    public string Answer { get; set; }
    public IActionResult OnGet(string answer)
    {
        if (!dayValidation.HasAccess2020(door))
        {
            return RedirectToPage("Index", new { invalidDoor = door });
        }
        if (string.IsNullOrEmpty(answer))
        {
            return Page();
        }
        if (string.Equals(answer, "dollhouse", StringComparison.InvariantCultureIgnoreCase))
        {
            Solved = true;
        }
        else if (CompareHelper.AreEqual(answer, "topuvdppuollhideorainoguasmee"))
        {
            ModelState.AddModelError(string.Empty, $"His code is not a real christmas whish. His wish must be encrypted inside this code.");
        }
        else
        {
            ModelState.AddModelError(string.Empty, $"No, {answer} is not the Christmas wish from Juan.");
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("Note15", new { answer = Answer });
    }
}
