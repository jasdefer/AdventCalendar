using AdventCalendarWebApp.Helper.TimeProvider;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace AdventCalendarWebApp.Pages.Helper
{
    public class SetCustomTimeModel : PageModel
    {
        public SetCustomTimeModel(ITimeProvider timeProvider)
        {
            TimeProvider = timeProvider;
        }

        public int? Index { get; set; }
        public ITimeProvider TimeProvider { get; set; }
        public void OnGet(int? index = null)
        {
            if(!index.HasValue)
            {
                return;
            }
            if(index<0 || index > 23)
            {
                index = 0;
            }
            DebugTimeProvider.DoorIndex = index.Value;
            Index = index;
        }
    }
}