using System;

namespace MyEPA.Helper
{
    public class CacheKeyHelper
    {
        public static string GetSMSLogin(string userName)
        {
            return $"GetSMSLogin_{userName}";
        }

        public static string GetCodeErrorKey(string userName,string verifyCode)
        {
            return $"GetCodeErrorKey_{userName}_{verifyCode}";
        }

        public static string GetSendSMSVerifyCountByDate(string userName)
        {
            var date = DateTimeHelper.GetCurrentTime().ToShortDateString();
            return $"GetSendCodeCountByDate_{userName}_{date}";
        }

        public static string GetSendSMSVerifyTime(string userName)
        {
            return $"GetSendSMSVerifyTime_{userName}";
        }

        public static string GetIsExistsRuning()
        {
            return $"GetIsExistsRuning";
        }
    }
}