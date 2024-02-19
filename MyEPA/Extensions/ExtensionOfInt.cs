using System;
using System.Text.RegularExpressions;

namespace MyEPA.Extensions
{
    public static class ExtensionOfInt
    {
        public static int? TryToInt(this string str)
        {
            int n = 0;
            if(int.TryParse(str, out n))
            {
                return n;
            }
            return null;
        }

        public static int? TryToInt(this object obj)
        {
            if(obj == null)
            {
                return null;
            }
            int n = 0;
            if (int.TryParse(obj.ToString(), out n))
            {
                return n;
            }
            return null;
        }

        public static bool? TryToBool(this string str)
        {
            bool n = false;
            if (bool.TryParse(str, out n))
            {
                return n;
            }
            return null;
        }

        public static string ToChineseNumber(this int number)
        {
            string sInput = number.ToString();

            string result = Regex.Replace(sInput, ".",
              delegate (Match m)
              {
                  return "０一二三四五六七八九"[m.Value[0] - '0'].ToString();
              });

            return result;
        }
    }
}