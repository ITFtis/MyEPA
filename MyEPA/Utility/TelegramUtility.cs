using System;
using System.Configuration;

namespace MyEPA.Utility
{
    public class TelegramUtility
    {
        private const string _settingKeyOfTelegramToken = "TelegramToken";
        private const string _settingKeyOfTelegramMessageEnableLevel = "SlackMessageEnableLevel";

        private static string _token;

        private static void InitializeConfigSetting()
        {
            if (string.IsNullOrWhiteSpace(_token) == false)
                return;

            _token = ConfigurationManager.AppSettings[_settingKeyOfTelegramToken];

            if (string.IsNullOrWhiteSpace(_token))
                _token = "bot1070688007:AAFunKLi6qOsAH_274gnvg8cImA4wFp_D8A";
        }


        public static void SendMessageToChannelAsync(TelegramMessageParameter parameter)
        {
            InitializeConfigSetting();

            //非同步
            Task.Factory.StartNew(() =>
            {
                SendMessage(parameter);
            });
        }

        public static void SendMessageToChannelNotAsync(TelegramMessageParameter parameter)
        {
            InitializeConfigSetting();

            //同步
            SendMessage(parameter);
        }

        public static void SendSlackMessageToChannelNotAsync(TelegramMessageParameter parameter)
        {
            InitializeConfigSetting();

            //同步
            SendTelegramMessage(parameter);
        }

        private static void SendMessage(TelegramMessageParameter parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter.Message))
                return;

            SendTelegramMessage(parameter);
        }

        public static void SendTelegramMessage(TelegramMessageParameter parameter)
        {
            SlackMessageEnableLevelEnum enableLevel = GetMessageEnableLevelSetting();

            //若網站設定 不顯示debugLevel等級的訊息 不做事情
            if (parameter.MessageLevel == SlackMessageLevelEnum.Debug &&
                enableLevel == SlackMessageEnableLevelEnum.ShowNormalMessageOnly)
            {
                return;
            }

            parameter.Message = parameter.Message.Replace("#", "＃");

            if (IsDevelopEnv())
            {
                parameter.Message = "【開發環境測試】" + parameter.Message;
            }

            string baseUrl = $"https://api.telegram.org/{_token}";
            string apiUrl = "sendMessage";

            WebRequestHelper
                .Request(baseUrl, apiUrl)
                .Get(e => e
                     .AddParameter("chat_id", parameter.Channel.ToInteger())
                     .AddParameter("text", parameter.Message)
                ).Response<string>();
        }

        private static bool IsDevelopEnv()
        {
            try
            {
                string configValue = ConfigurationManager.AppSettings[_settingKeyOfIsDevelopEnv];

                if (string.IsNullOrWhiteSpace(configValue))
                    return false;

                bool isLocalHost = bool.Parse(configValue);
                return isLocalHost;
            }
            catch
            {

            }

            return false;
        }

        private static SlackMessageEnableLevelEnum GetMessageEnableLevelSetting()
        {
            try
            {
                string configValue = ConfigurationManager.AppSettings[_settingKeyOfTelegramMessageEnableLevel];

                if (string.IsNullOrWhiteSpace(configValue))
                    return SlackMessageEnableLevelEnum.ShowNormalMessageOnly;

                int parse = Int32.Parse(configValue);
                return (SlackMessageEnableLevelEnum)parse;
            }
            catch
            {

            }

            return SlackMessageEnableLevelEnum.ShowNormalMessageOnly;
        }
    }
}