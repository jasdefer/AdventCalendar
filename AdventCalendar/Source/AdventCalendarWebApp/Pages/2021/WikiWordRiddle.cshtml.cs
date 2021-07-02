using AdventCalendarWebApp.Helper.Adventia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace AdventCalendarWebApp.Pages._2021
{
    public class WikiWordRiddleModel : PageModel
    {
        public string[] Words { get; set; }
        
        [BindProperty]
        public string Guess { get; set; }

        [BindProperty]
        public int Seed { get; set; }

        public string Message { get; set; }
        [BindProperty]
        public int Number { get; set; }

        public async Task OnGet(int number, int seed, string message)
        {
            Number = number;
            Message = message;
            Seed = seed;
            string text;
            var keyword = Wikipedia.GetRandomKeyword(Seed);
            try
            {
                text = await Wikipedia.GetTextAsync(keyword);
            }
            catch (Exception)
            {
                Message = "Invalid keyword";
                return;
            }
            Words = WordSelection.GetWords(text, Number, new Random(1), WordSelection.GermanBlacklist, keyword);
        }

        public IActionResult OnPost()
        {
            var keyword = Wikipedia.GetRandomKeyword(Seed);
            if(keyword == Guess)
            {
                Message = "Correct";
            }
            else
            {
                Message = "Incorrect";
                Number++;
            }
            return RedirectToPage("WikiWordRiddle", new { message = Message, seed = Seed, number = Number });
        }
    }
}
