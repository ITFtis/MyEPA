using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MyEPA.Helper
{
    public class SettingHelper
    {
        public static string CarSystemUrl
        {
            get
            {
                return GetAppSetting<string>("CarSystemUrl", "https://newemis.epa.gov.tw/carmgt/dispatch.aspx");
            }
        }

        public static bool IsDeBug
        {
            get
            {
                return GetAppSetting<bool>("isDeBug", false);
            }
        }
        public static int SMSVerifyMaxCount
        {
            get
            {
                return GetAppSetting<int>("SMSVerifyMaxCount", 3);
            }
        }
        public static int MaxInputSMSCount
        {
            get
            {
                return GetAppSetting<int>("MaxInputSMSCount", 5);
            }
        }

        public static int SMSVerifyTimeLimitMinutes
        {
            get
            {
                return GetAppSetting<int>("SMSVerifyTimeLimitMinutes", 15);
            }
        }

        public static T GetAppSetting<T>(string key, T defaultValue)
        {
            return SettingExtension.GetAppSetting<T>(key, defaultValue);
        }
    }
    internal class SettingExtension
    {
        public static T GetAppSetting<T>(string key)
        {
            var stringValue = ConfigurationManager.AppSettings[key];

            if (string.IsNullOrWhiteSpace(stringValue))
                throw new Exception("miss settingConfig: " + key);

            return (T)Convert.ChangeType(stringValue, typeof(T));
        }


        public static T GetAppSetting<T>(string key, T defaultValue)
        {
            var stringValue = ConfigurationManager.AppSettings[key];

            if (string.IsNullOrWhiteSpace(stringValue))
                return defaultValue;

            return (T)Convert.ChangeType(stringValue, typeof(T));
        }

    }
}