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
            var page = await Wikipedia.GetTextAsync("Basketball");
            var words = WordSelection.GetWords(page.extract, 50, new Random(1), WordSelection.GermanBlacklist, "basketball");
        }   
    }
}
