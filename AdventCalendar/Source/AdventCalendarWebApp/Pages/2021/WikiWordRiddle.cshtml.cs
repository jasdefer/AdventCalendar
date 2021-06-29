using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventCalendarWebApp.Helper.Adventia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2021
{
    public class WikiWordRiddleModel : PageModel
    {
        public string[] Words { get; set; }
        
        [BindProperty]
        public string Guess { get; set; }

        [BindProperty]
        public string Keyword { get; set; }

        public string Message { get; set; }
        [BindProperty]
        public int Number { get; set; }

        public async Task OnGet(int number, string keyword, string message)
        {
            Keyword = keyword.ToLowerInvariant();
            Number = number;
            Message = message;
            string text;
            try
            {
                text = await Wikipedia.GetTextAsync(Keyword);
            }
            catch (Exception)
            {
                Message = "Invalid keyword";
                return;
            }
            Words = WordSelection.GetWords(text, Number, new Random(1), WordSelection.GermanBlacklist, Keyword);
        }

        public IActionResult OnPost()
        {
            if(Keyword == Guess)
            {
                Message = "Correct";
            }
            else
            {
                Message = "Incorrect";
                Number++;
            }
            return RedirectToPage("WikiWordRiddle", new { message = Message, keyword = Keyword, number = Number });
        }
    }
}
