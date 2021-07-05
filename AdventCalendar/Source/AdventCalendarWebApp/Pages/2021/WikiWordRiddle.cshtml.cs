using AdventCalendarWebApp.Helper.Adventia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;
using static AdventCalendarWebApp.Helper.Adventia.Wikipedia;

namespace AdventCalendarWebApp.Pages._2021
{
    public class WikiWordRiddleModel : PageModel
    {
        public string[] Words { get; set; }
        public string[] Categories { get; set; }

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
            SingleArticlePage page;
            var keyword = Wikipedia.GetRandomKeyword(Seed);
            try
            {
                page = await Wikipedia.GetTextAsync(keyword);
                Categories = page.categories
                    .Select(x => x.title
                        .Replace("Kategorie:", "")
                        .ToLowerInvariant())
                    .Where(x => x!=keyword)
                    .ToArray();
            }
            catch (Exception)
            {
                Message = "Invalid keyword";
                return;
            }
            Words = WordSelection.GetWords(page.extract, Number, new Random(1), WordSelection.GermanBlacklist, keyword);
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
