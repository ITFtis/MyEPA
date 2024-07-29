using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA
{
    public class PwdHelper
    {
        public static string ErrorMessage
        {
            set;  get;
        }

        public static bool ValidPassword(string password)
        {
            ErrorMessage = "";

            if (string.IsNullOrWhiteSpace(password) || password.Length < 12)
            {
                ErrorMessage = "抱歉，密碼長度須達 12 碼以上";
                return false;
            }

            return true;
        }
    }
}