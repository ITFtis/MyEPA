using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MyEPA.Helper
{
    public static class RegexHelper
    {
        public static bool IsMobile(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return false;
            
            return Regex.IsMatch(mobile, RegexUtility.Mobile) 
                || Regex.IsMatch(mobile, RegexUtility.Mobile2) 
                || Regex.IsMatch(mobile, RegexUtility.Mobile3);
        }

        public static bool IsNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return false;

            return System.Text.RegularExpressions.Regex.IsMatch(number, RegexUtility.Number);
        }
    }
}