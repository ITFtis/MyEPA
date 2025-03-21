﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MyEPA
{
    public class DateFormat
    {
        public static DateTime MinDate {
            get
            {
                return DateTime.Parse("1911/01/01");
            }
        }

        /// <summary>
        /// 字串轉日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(string date)
        {
            DateTime result = DateTime.MinValue;

            DateTime.TryParse(date, out result);

            return result;
        }

        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate1(string date)
        {
            string result = "";

            try
            {
                DateTime dd = DateTime.Parse(date);
                result = ToDate1(dd);
            }
            catch
            {
                return date;
            }

            return result;
        }

        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate1(DateTime date)
        {
            string result = "";

            result = date.ToString("yyyy-MM-dd");

            return result;
        }

        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate2(string date)
        {
            string result = "";

            try
            {
                DateTime dd = DateTime.Parse(date);
                result = ToDate2(dd);
            }
            catch
            {
                return date;
            }

            return result;
        }

        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate2(DateTime date)
        {
            string result = "";

            result = date.ToString("yyyy-MM-dd HH:mm:ss");

            return result;
        }

        /// <summary>
        /// MM
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate3(string date)
        {
            string result = "";

            try
            {
                DateTime dd = DateTime.Parse(date);
                result = ToDate3(dd);
            }
            catch
            {
                return date;
            }

            return result;
        }

        /// <summary>
        /// MM
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate3(DateTime date)
        {
            string result = "";

            result = date.ToString("MM");

            return result;
        }

        /// <summary>
        /// yyyy/MM/dd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate4(string date)
        {
            string result = "";

            try
            {
                DateTime dd = DateTime.Parse(date);
                result = ToDate4(dd);
            }
            catch
            {
                return date;
            }

            return result;
        }

        /// <summary>
        /// yyyy/MM/dd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate4(DateTime date)
        {
            string result = "";

            //當 datetime 為最小值時，回傳「空字串」。
            if (date == DateTime.MinValue)
                result = "";
            else
                result = date.ToString("yyyy/MM/dd");

            return result;
        }

        /// <summary>
        /// yyyy/MM
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate5(string date)
        {
            string result = "";

            try
            {
                DateTime dd = DateTime.Parse(date);
                result = ToDate5(dd);
            }
            catch
            {
                return date;
            }

            return result;
        }

        /// <summary>
        /// yyyy/MM
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate5(DateTime date)
        {
            string result = "";

            result = date.ToString("yyyy/MM");

            return result;
        }

        /// <summary>
        /// yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate6(string date)
        {
            string result = "";

            try
            {
                DateTime dd = DateTime.Parse(date);
                result = ToDate6(dd);
            }
            catch
            {
                return date;
            }

            return result;
        }

        /// <summary>
        /// yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate6(DateTime date)
        {
            string result = "";

            result = date.ToString("yyyy-MM-dd HH:mm");

            return result;
        }

        /// <summary>
        /// yyyy/MM/dd HH:mm
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate7(object date)
        {
            string result = "";

            if (date == null)
                return "";

            try
            {
                DateTime dd = DateTime.Parse(date.ToString());
                result = string.Format("{0:yyyy/MM/dd HH:mm}", dd);
            }
            catch
            {
                return null;
            }

            return result;
        }

        /// <summary>
        /// yyyy/MM/dd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate7_2(object date)
        {
            string result = "";

            if (date == null)
                return "";

            try
            {
                DateTime dd = DateTime.Parse(date.ToString());
                result = string.Format("{0:yyyy/MM/dd}", dd);
            }
            catch
            {
                return null;
            }

            return result;
        }

        /// <summary>
        /// yyyyMMdd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate8(string date)
        {
            string result = "";

            try
            {
                DateTime dd = DateTime.Parse(date);
                result = ToDate4(dd);
            }
            catch
            {
                return date;
            }

            return result;
        }

        /// <summary>
        /// yyyyMMdd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate8(DateTime date)
        {
            string result = "";

            //當 datetime 為最小值時，回傳「空字串」。
            if (date == DateTime.MinValue)
                result = "";
            else
                result = date.ToString("yyyyMMdd");

            return result;
        }

        /// <summary>
        /// yyyyMM
        /// </summary>
        /// <param name="date"></param>
        /// <returns>string</returns>
        public static string ToDate9(DateTime date)
        {
            string result = "";

            //當 datetime 為最小值時，回傳「空字串」。
            if (date == DateTime.MinValue)
                result = "";
            else
                result = date.ToString("yyyyMM");

            return result;
        }

        /// <summary>
        /// yyyyMM 
        /// </summary>
        /// <param name="date"></param>
        /// <returns>DateTime</returns>
        public static DateTime ToDate10(string date)
        {
            DateTime result = DateTime.MinValue;

            try
            {
                string y = date.Substring(0, 4);
                string m = date.Substring(4, 2);
                string d = "01";

                DateTime.TryParse(y + "/" + m + "/" + d, out result);
            }
            catch (Exception ex)
            {
                return result;
            }

            return result;
        }

        /// <summary>
        /// MM/dd (一)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate11(string date)
        {
            string result = "";

            try
            {
                DateTime dd = DateTime.Parse(date);
                result = ToDate11(dd);
            }
            catch
            {
                return date;
            }

            return result;
        }

        /// <summary>
        /// MM/dd (一)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate11(DateTime date)
        {
            string result = "";

            //當 datetime 為最小值時，回傳「空字串」。
            if (date == DateTime.MinValue)
                result = "";
            else
            {
                string str = "";
                switch (date.DayOfWeek.ToString())
                {
                    case "Sunday":
                        str = "(日)";
                        break;
                    case "Monday":
                        str = "(一)";
                        break;
                    case "Tuesday":
                        str = "(二)";
                        break;
                    case "Wednesday":
                        str = "(三)";
                        break;
                    case "Thursday":
                        str = "(四)";
                        break;
                    case "Friday":
                        str = "(五)";
                        break;
                    case "Saturday":
                        str = "(六)";
                        break;
                }

                result = date.ToString("MM/dd") + " " + str;
            }

            return result;
        }

        /// <summary>
        /// HH:mm
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate12(string date)
        {
            string result = "";

            try
            {
                DateTime dd = DateTime.Parse(date);
                result = ToDate12(dd);
            }
            catch
            {
                return date;
            }

            return result;
        }

        /// <summary>
        /// HH:mm
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate12(DateTime date)
        {
            string result = "";

            result = date.ToString("HH:mm");

            return result;
        }

        /// <summary>
        /// yyyy-MM-dd HH
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate13(string date)
        {
            string result = "";

            try
            {
                DateTime dd = DateTime.Parse(date);
                result = ToDate13(dd);
            }
            catch
            {
                return date;
            }

            return result;
        }

        /// <summary>
        /// yyyy年MM月dd日 HH點
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate13(DateTime date)
        {
            string result = "";

            //2024年2月27日上午8點
            result = string.Format("{0}年{1}月{2}日{3}點", date.Year, date.Month, date.Day, date.Hour);

            return result;
        }

        /// <summary>
        /// 西元轉民國：2004-12-20 00:00:00.000 => 091/01/01
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToTwDate1(string date)
        {
            string result = "";

            try
            {
                DateTime dd = DateTime.Parse(date);
                result = ToTwDate1(dd);
            }
            catch
            {
                return date;
            }

            return result;
        }

        /// <summary>
        /// 西元轉民國：2004-12-20 00:00:00.000 => 091/01/01
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToTwDate1(DateTime date)
        {
            string result = "";

            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            result = date.ToString("yyy", culture).PadLeft(3, '0') + date.ToString("/MM/dd", culture);

            return result;
        }

        /// <summary>
        /// 西元轉民國：2004-12-20 00:00:00.000 => 112 或 091
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToTwDate2(DateTime date)
        {
            string result = "";

            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            string str = date.ToString("yyy", culture).PadLeft(3, '0');

            if (str.Substring(0, 1) == "0")
            {
                result = str.Substring(1, 2);
            }
            else
            {
                result = str;
            }

            return result;
        }

        /// <summary>
        /// 西元轉民國：200403 => 112.03 或 091.08
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToTwDate3(string date)
        {
            string result = date;

            try
            {
                string y = date.Substring(0, 4);
                string m = date.Substring(4, 2);
                string d = "01";

                DateTime t = DateTime.MinValue;
                if (DateTime.TryParse(y + "/" + m + "/" + d, out t))
                {
                    string a = (t.Year - 1911).ToString().PadLeft(3, '0');
                    string b = m;
                    result = a + "." + b;
                }
            }
            catch (Exception ex)
            {
                return result;
            }

            return result;
        }

        /// <summary>
        /// 西元轉民國：2023-12-20 00:00:00.000 => 112年3月5日
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToTwDate4(string date)
        {
            string result = "";

            try
            {
                DateTime dd = DateTime.Parse(date);
                result = ToTwDate4(dd);
            }
            catch
            {
                return date;
            }

            return result;
        }

        /// <summary>
        /// 西元轉民國：2023-12-20 00:00:00.000 => 112年3月5日
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToTwDate4(DateTime date)
        {
            string result = "";

            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            //result = date.ToString("yyy", culture).PadLeft(3, '0') + date.ToString("/MM/dd", culture);

            string str = date.ToString("yyy", culture).PadLeft(3, '0');

            if (str.Substring(0, 1) == "0")
            {
                result = string.Format("{0}年{1}月{2}日", str.Substring(1, 2), date.Month, date.Day);
            }
            else
            {
                result = string.Format("{0}年{1}月{2}日", str, date.Month, date.Day);
            }

            return result;
        }

        /// <summary>
        /// 西元轉民國：2023-12-20 00:00:00.000 => 112/3/5
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToTwDate5(string date)
        {
            string result = "";

            try
            {
                DateTime dd = DateTime.Parse(date);
                result = ToTwDate5(dd);
            }
            catch
            {
                return date;
            }

            return result;
        }

        /// <summary>
        /// 西元轉民國：2023-12-20 00:00:00.000 => 112/3/5
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToTwDate5(DateTime date)
        {
            string result = "";

            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            //result = date.ToString("yyy", culture).PadLeft(3, '0') + date.ToString("/MM/dd", culture);

            string str = date.ToString("yyy", culture).PadLeft(3, '0');

            if (str.Substring(0, 1) == "0")
            {
                result = string.Format("{0}/{1}/{2}", str.Substring(1, 2), date.Month, date.Day);
            }
            else
            {
                result = string.Format("{0}/{1}/{2}", str, date.Month, date.Day);
            }

            return result;
        }

        /// <summary>
        /// 民國轉西元：091/01/01 => 2004-12-20 00:00:00.000
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToBCDate1(string date)
        {
            DateTime result = DateTime.MinValue;

            try
            {
                CultureInfo culture = new CultureInfo("zh-TW");
                culture.DateTimeFormat.Calendar = new System.Globalization.TaiwanCalendar();
                result = DateTime.Parse(date, culture);
            }
            catch (Exception ex)
            {
                result = DateTime.MinValue;
            }

            return result;
        }

        /// <summary>
        /// 民國轉西元：112年1月7日(六) => 2004-12-20 00:00:00.000
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToBCDate2(string date)
        {
            DateTime result = DateTime.MinValue;

            try
            {
                date = date.Split('日')[0].Replace('年', '/').Replace('月', '/');
                result = ToBCDate1(date);
            }
            catch (Exception ex)
            {
                result = DateTime.MinValue;
            }

            return result;
        }

        /// <summary>
        /// 民國轉西元：2023-05-08 (一) => 2004-12-20 00:00:00.000
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToBCDate3(string date)
        {
            DateTime result = DateTime.MinValue;

            try
            {
                date = date.Split(' ')[0];
                result = DateTime.Parse(date);
            }
            catch (Exception ex)
            {
                result = DateTime.MinValue;
            }

            return result;
        }

        /// <summary>
        /// 西元字串(yyyyMMdd)轉西元日期：20210715 => 2021/07/15 00:00:00.000
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToBCDate4(string date)
        {
            DateTime result = DateTime.MinValue;

            try
            {
                string str = date.Substring(0, 4) + "/" + date.Substring(4, 2) + "/" + date.Substring(6, 2);

                if (!DateTime.TryParse(str, out result))
                {
                    return DateTime.MinValue;
                }
                else
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                result = DateTime.MinValue;
            }

            return result;
        }

        /// <summary>
        /// 01:02
        /// </summary>
        /// <param name="time">01:02:00</param>
        /// <returns></returns>
        public static string ToTime1(string str)
        {
            string result = "";

            TimeSpan time;
            if (TimeSpan.TryParse(str, out time))
            {
                result = time.ToString(@"hh\:mm");
            }
            else
            {
                result = str;
            }

            return result;
        }

        /// <summary>
        /// 天數差異(n天)
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static int ToDiffDays(DateTime? date1, DateTime date2)
        {
            if (date1 == null)
                return 0;

            return ToDiffDays((DateTime)date1, date2);
        }

        /// <summary>
        /// 天數差異(n天)
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static int ToDiffDays(DateTime? date1, DateTime? date2)
        {
            return ToDiffDays((DateTime)date1, (DateTime)date2);
        }

        /// <summary>
        /// 天數差異(n天)
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static int ToDiffDays(DateTime date1, DateTime date2)
        {
            int result = -1;

            try
            {
                DateTime d1 = DateTime.Parse(date1.ToShortDateString());
                DateTime d2 = DateTime.Parse(date2.ToShortDateString());

                result = (d1 - d2).Days;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}