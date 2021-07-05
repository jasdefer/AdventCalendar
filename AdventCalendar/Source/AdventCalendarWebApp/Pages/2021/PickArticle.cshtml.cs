using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventCalendarWebApp.Helper.Adventia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventCalendarWebApp.Pages._2021
{
    public class PickArticleModel : PageModel
    {
        private const int NumberOfWords = 5;
        private const int NumberOfTitles = 7;

        [BindProperty]
        public IReadOnlyList<string> Titles { get; set; }
        [BindProperty]
        public int CorrectIndex { get; set; }
        [BindProperty]
        public List<string> Words { get; set; }
        [BindProperty]
        public int Answer { get; set; }
        public string Message { get; set; }

        public async Task OnGet(List<string> titles, List<string> words, int correctIndex, int answer)
        {
            if (titles == null ||
                titles.Count == 0)
            {

                Titles = await Wikipedia.GetRandomTitle(NumberOfTitles);
                var random = new Random();
                CorrectIndex = random.Next(0, Titles.Count);
                var page = await Wikipedia.GetTextAsync(Titles[CorrectIndex]);
                Words = WordSelection.GetWords(page.extract, NumberOfWords, random, WordSelection.GermanBlacklist, Titles[CorrectIndex]).ToList();
            }
            else
            {
                Titles = titles;
                CorrectIndex = correctIndex;
                Words = words;
                Answer = answer;
                if(answer == CorrectIndex)
                {
                    Message = "Correct";
                }
                else
                {
                    Message = "Incorrect";
                }
            }
            
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("PickArticle", new { titles = Titles, words = Words, correctIndex = CorrectIndex, answer = Answer });
        }
    }
}