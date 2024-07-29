using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

            if (password == null)
            {
                ErrorMessage = "未提供密碼";
                return false;
            }

            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(password) || password.Length < 12)
            {
                errors.Add("密碼長度須達 12 碼以上");
            }

            string patternEN = @"^(?=.*[a-zA-Z])(?=.*\d).+$";
            Regex regEN = new Regex(patternEN);
            if (!regEN.IsMatch(password))
            {
                errors.Add("密碼需包含1個英文及1個數字");
            }

            if (errors.Count > 0)
            {
                ErrorMessage = "抱歉：" + string.Join("、", errors);
                return false;
            }

            return true;
        }
    }
}