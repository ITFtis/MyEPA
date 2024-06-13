using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace MyEPA
{
    public class AppConfig
    {
        #region 私有變數

        private static string _DEDSWebSite;

        private static string _htmlTemplatePath;
        private static string _emailAddress;
        private static string _emailFromName;
        private static string _emailPassword;
        private static string _smtpServer;
        private static int _smtpPort;
        private static bool _enableSSL;
        private static bool _sendEmail;
        private static string _testEmailAddress;
        private static string _EmailAddressCC;

        #endregion

        #region 建構子

        static AppConfig()
        {
            _DEDSWebSite = ConfigurationManager.AppSettings["DEDSWebSite"].ToString();

            _htmlTemplatePath = ConfigurationManager.AppSettings["HtmlTemplatePath"].ToString();
            _emailAddress = ConfigurationManager.AppSettings["EmailAddress"].ToString();
            _emailPassword = ConfigurationManager.AppSettings["EmailPassword"].ToString();
            _emailFromName = ConfigurationManager.AppSettings["EmailFromName"].ToString();

            _smtpServer = ConfigurationManager.AppSettings["SMTPServer"].ToString();
            _smtpPort = int.Parse(ConfigurationManager.AppSettings["SMTPPort"].ToString());
            _sendEmail = ConfigurationManager.AppSettings["SendEmail"].ToString() == "Y" ? true : false;
            _testEmailAddress = ConfigurationManager.AppSettings["TestEmailAddress"].ToString();
            _EmailAddressCC = ConfigurationManager.AppSettings["EmailAddressCC"].ToString();
            _enableSSL = ConfigurationManager.AppSettings["EnableSSL"].ToString() == "Y" ? true : false;
        }

        #endregion

        #region 公用屬性        

        /// <summary>
        ///網站網址
        /// </summary>
        public static string DEDSWebSite
        {
            get { return _DEDSWebSite; }
        }

        /// <summary>
        ///Html樣式路徑
        /// </summary>
        public static string HtmlTemplatePath
        {
            get { return _htmlTemplatePath; }
        }

        /// <summary>
        /// Email Address
        /// </summary>
        public static string EmailAddress
        {
            get { return _emailAddress; }
        }

        /// <summary>
        /// Email寄件者名稱
        /// </summary>
        public static string EmailFromName
        {
            get { return _emailFromName; }
        }

        /// <summary>
        /// Email 密碼
        /// </summary>
        public static string EmailPassword
        {
            get
            {

                ////if (_isDesEncode == "Y")
                ////{
                ////    _emailPassword = GetDesDecode(_emailPassword);
                ////}

                return _emailPassword;
            }
        }

        /// <summary>
        /// SMTP
        /// </summary>
        public static string SMTPServer
        {
            get { return _smtpServer; }
        }

        /// <summary>
        /// SMTP Port
        /// </summary>
        public static int SMTPPort
        {
            get { return _smtpPort; }
        }

        /// <summary>
        /// email server是否使用SSL
        /// </summary>
        public static bool EnableSSL
        {
            get { return _enableSSL; }
        }

        /// <summary>
        /// 是否要寄發Email
        /// </summary>
        public static bool SendEmail
        {
            get { return _sendEmail; }
        }

        /// <summary>
        /// 測試用的收件者Email
        /// </summary>
        public static string TestEmailAddress
        {
            get { return _testEmailAddress; }
        }

        /// <summary>
        /// 測試用的副本Email 
        /// </summary>
        public static string EmailAddressCC
        {
            get { return _EmailAddressCC; }
        }

        #endregion
    }
}