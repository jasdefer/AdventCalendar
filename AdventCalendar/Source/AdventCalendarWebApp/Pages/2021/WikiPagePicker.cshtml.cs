using AdventCalendarWebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AdventCalendarWebApp.Pages._2021
{
    public class WikiPagePickerModel : PageModel
    {
        private readonly DayValidation dayValidation;


        public List<SelectListItem> Options { set; get; }

        [BindProperty]
        public int Answer { get; set; }

        public WikiPagePickerModel(DayValidation dayValidation)
        {
            this.dayValidation = dayValidation;
        }

        public IActionResult OnGet(int day, int? answer)
        {
            var hasAccess = dayValidation.HasAccess2020(day);
            if (!hasAccess)
            {
                return RedirectToPage("Index", new { invalidDoor = day });
            }
            if (!answer.HasValue)
            {
                return Page();
            }
            return Page();
        }

        private void SetupOptions(int day)
        {
            Options = new List<SelectListItem>();
        }

        public IActionResult OnPost(int day)
        {
            return RedirectToPage("WikiPagePicker", new { day = day, answer = Answer });
        }
    }
}