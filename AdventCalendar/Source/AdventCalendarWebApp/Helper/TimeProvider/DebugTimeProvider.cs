﻿using System;

namespace AdventCalendarWebApp.Helper.TimeProvider
{
    public class DebugTimeProvider : ITimeProvider
    {
        public static int DoorIndex = 24;
        public DateTime Now()
        {
            return Dates.Doors2020[DoorIndex];
        }
    }
}