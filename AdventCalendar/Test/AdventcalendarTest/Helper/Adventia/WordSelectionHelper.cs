using AdventCalendarWebApp.Helper.Adventia;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventcalendarTest.Helper.Adventia
{
    [TestClass]
    public class WordSelectionHelper
    {
        [TestMethod]
        public void CleanupTest()
        {
            var result = WordSelection.Cleanup("1234567890-_)(*&^%$#@!,./;'\\][<>?:\"|}{ÖÄÜöäüßqwertzuioplkjhgfdsayxcvbnmQWERTZUIOPLKJHGFDSAYXCVBNM");
            Assert.AreEqual(1+26 + 26 + 7, result.Length);//1 for the period that converts to a space
        }
    }
}
