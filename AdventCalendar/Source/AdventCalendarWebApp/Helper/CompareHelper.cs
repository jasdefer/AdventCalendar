using System;
using System.Collections.Generic;

namespace AdventCalendarWebApp.Helper
{
    public static class CompareHelper
    {
        public static bool AreEqual(string s1, string s2)
        {
            return string.Equals(s1, s2, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool Contains(IEnumerable<string> collection, string input)
        {
            if (collection == null)
            {
                return false;
            }
            foreach (var item in collection)
            {
                if (AreEqual(item, input))
                {
                    return true;
                }
            }
            return false;
        }
    }
}