using System;
using System.Text.RegularExpressions;

namespace MyEPA.Extensions
{
    public static class ExtensionOfString
    {
        public static string ChangToUrlLink(this string input)
        {
            string result = string.Empty;
            if (Regex.Match(input, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?") != Match.Empty)
            {
                result = Regex.Replace(input, @"(?<link>http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)+", "<a href=\"${link}\">${link}</a>");
            }
            return result;
        }
        public static bool IsJArray(this string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                json = json.Trim('\r', '\n', ' ');
                if (json.Length > 1)
                {
                    char s = json[0];
                    char e = json[json.Length - 1];
                    return (s == '[' && e == ']');
                }
            }
            return false;
        }
        public static Nullable<T> TryToEnum<T>(this string str) where T : struct
        {
            T result = default(T);
            if (Enum.TryParse(str, true, out result))
            {
                return result;
            }
            return null;
        }

    }
}