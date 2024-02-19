using System;

namespace MyEPA.Extensions
{
    public static class ExtensionOfDatetime
    {
        public static DateTime To_23_59_59(this DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, 23, 59, 59, 999);
        }

        public static DateTime To_00_00_00(this DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
        }
        public static string ToDateTimeString(this DateTime time, string format = "yyyy/MM/dd HH:mm")
        {
            return ToDateTimeString((DateTime?)time, format);
        }
        public static string ToDateTimeString(this DateTime? time,string format = "yyyy/MM/dd HH:mm")
        {
            if(time.HasValue == false)
            {
                return string.Empty;
            }
            return time.Value.ToString(format);
        }
        public static string ToDateString(this DateTime date, string format = "yyyy/MM/dd")
        {
            return ToDateString((DateTime?)date, format);
        }
        public static string ToDateString(this DateTime? date, string format = "yyyy/MM/dd")
        {
            if (date.HasValue == false)
            {
                return string.Empty;
            }
            return date.Value.ToString(format);
        }
        public static DateTime? TryToDateTime(this string str)
        {
            DateTime date;
            if (DateTime.TryParse(str, out date))
                return date;

            return null;
        }

        public static decimal? TryToDecimal(this string str)
        {
            decimal date;
            if (decimal.TryParse(str, out date))
                return date;

            return null;
        }
    }
}