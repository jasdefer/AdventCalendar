using AdventCalendarWebApp.Helper.Adventia;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventcalendarTest.Helper.Adventia
{
    [TestClass]
    public class WikipediaTest
    {
        [TestMethod]
        public async Task MyTestMethod()
        {
            var text = await Wikipedia.GetTextAsync("Basketball");
            var words = WordSelection.GetWords(text, 50, new Random(1), WordSelection.GermanBlacklist, "basketball");
        }   
    }
}
