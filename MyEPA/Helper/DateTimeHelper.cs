using System;
using System.Collections.Generic;

namespace MyEPA.Helper
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Get DateTime.UtcNow.AddHours(8)
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentTime(int timeOffset = 8)
        {
            return DateTime.UtcNow.AddHours(timeOffset);
        }

        public static List<DateTime> GetBetweenAllDates(DateTime startTime, DateTime endTime)
        {
            List<DateTime> result = new List<DateTime>();

            for (DateTime date= startTime.Date; date <= endTime.Date; date = date.AddDays(1))
            {
                result.Add(date);
            }

            return result;
        }

    }
}