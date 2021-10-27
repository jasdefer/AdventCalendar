using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;

namespace AdventCalendarWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private static int[] DaysOrdered = Enumerable.Range(1, 24).ToArray();
        public int[] Days { get; private set; }
        public void OnGet()
        {
            var random = new Random();
            Days = DaysOrdered.OrderBy(x => random.NextDouble()).ToArray();
        }
    }
}