using AdventCalendarWebApp.Helper.Adventia;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AdventcalendarTest.Helper.Adventia
{
    [TestClass]
    public class WordSelectionHelper
    {
        [TestMethod]
        public void CleanupTest()
        {
            var result = WordSelection.Cleanup("1234567890-_)(*&^%$#@!,./;'\\][<>?:\" |}{ÖÄÜöäüßqwertzuioplkjhgfdsayxcvbnmQWERTZUIOPLKJHGFDSAYXCVBNM");
            Assert.AreEqual(26 + 26 + 7, result.Length);
        }
    }
}
