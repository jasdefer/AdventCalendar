using AdventCalendarWebApp.Helper;
using AdventCalendarWebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace AdventCalendarWebApp.Pages._2021
{
    public class WikiPagePickerModel : PageModel
    {
        private readonly DayValidation dayValidation;
        public static readonly IReadOnlyList<IReadOnlyList<string>> OptionStrings = new string[12][]
        {
            new string[5]{"Neil Armstrong","Tour de France","Savanna","Helium","Jazz"},
            new string[]{"Korean War","second group","dangerous","broadcast","victory"},
            new string[]{"","","","",""},
            new string[]{"","","","",""},
            new string[]{"","","","",""},
            new string[]{"","","","",""},
            new string[]{"","","","",""},
            new string[]{"","","","",""},
            new string[]{"","","","",""},
            new string[]{"","","","",""},
            new string[]{"","","","",""},
            new string[]{"","","","",""},
        };

        public static readonly int[] CorrectOptions = new int[12]
        {
            1,1,1,1,1,1,1,1,1,1,1,1
        };

        public static IReadOnlyList<IReadOnlyList<string>> Words = new string[12][]
        {
            new string[]{ "word1","word2","word3" }, 
            new string[]{ },
            new string[]{ },
            new string[]{ },
            new string[]{ },
            new string[]{ },
            new string[]{ },
            new string[]{ },
            new string[]{ },
            new string[]{ },
            new string[]{ },
            new string[]{ }
        };

        public List<SelectListItem> Options { set; get; }
        public int Index => Day / 2 - 1;
        public int Day { get; set; }
        public ValidationState ValidationState { get; private set; } = ValidationState.NotValidated;

        public int? Answer { get; set; }
        public int NumberOfGuesses { get; set; }
        public DateTime StartOfGuessing { get; set; }
        public TimeSpan SolveDuration => DateTime.UtcNow - StartOfGuessing;

        public WikiPagePickerModel(DayValidation dayValidation)
        {
            this.dayValidation = dayValidation;
        }

        public IActionResult OnGet(int day,
            int? answer,
            int numberOfGuesses = 0,
            DateTime? startOfGuessing = null)
        {
            var hasAccess = dayValidation.HasAccess2020(day);
            if (!hasAccess)
            {
                return RedirectToPage("Index", new { invalidDoor = day });
            }
            Day = day;
            StartOfGuessing = startOfGuessing ?? DateTime.UtcNow;
            SetupOptions(Index);
            if (!answer.HasValue)
            {
                return Page();
            }
            NumberOfGuesses = ++numberOfGuesses;
            Answer = answer;
            ValidationState = answer.Value == CorrectOptions[Index] ? ValidationState.Correct : ValidationState.Incorrect;
            return Page();
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
}