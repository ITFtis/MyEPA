using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace EPASchedule
{
    internal class AppConfig
    {
        #region 私有變數

        private static string _rootPath;
        private static int _validDay;
        private static string _testEmailAddress;
        private static string _emailAddressCC;
        private static string _emailAddressResp;
        private static string _emailAddressGov;
        private static string _onlyStep;

        #endregion

        #region 建構子

        static AppConfig()
        {
            _rootPath = ConfigurationManager.AppSettings["RootPath"].ToString();
            _validDay = ConfigurationManager.AppSettings["ValidDay"] == null ? 0 : int.Parse(ConfigurationManager.AppSettings["ValidDay"].ToString());
            _testEmailAddress = ConfigurationManager.AppSettings["TestEmailAddress"].ToString();
            _emailAddressCC = ConfigurationManager.AppSettings["EmailAddressCC"] == null ? "" : ConfigurationManager.AppSettings["EmailAddressCC"].ToString();
            _emailAddressResp = ConfigurationManager.AppSettings["EmailAddressResp"] == null ? "" : ConfigurationManager.AppSettings["EmailAddressResp"].ToString();
            _emailAddressGov = ConfigurationManager.AppSettings["EmailAddressGov"] == null ? "" : ConfigurationManager.AppSettings["EmailAddressResp"].ToString();
            _onlyStep = ConfigurationManager.AppSettings["OnlyStep"].ToString();
        }

        #endregion

        #region 公用屬性        

        /// <summary>
        /// 檔案存放跟目錄
        /// </summary>
        public static string RootPath
        {
            get { return _rootPath; }
        }

        /// <summary>
        ///警示天數(ex：90 有效天數90天內的全部通知)
        /// </summary>
        public static int ValidDay
        {
            get { return _validDay; }
        }

        /// <summary>
        /// 測試用的收件者Email
        /// </summary>
        public static string TestEmailAddress
        {
            get { return _testEmailAddress; }
        }

        /// <summary>
        /// Email副本 
        /// </summary>
        public static string EmailAddressCC
        {
            get { return _emailAddressCC; }
        }

        /// <summary>
        /// Email負責人(淑俐) 
        /// </summary>
        public static string EmailAddressResp
        {
            get { return _emailAddressResp; }
        }

        /// <summary>
        /// 環衛組與綜規組
        /// </summary>
        public static string EmailAddressGov
        {
            get { return _emailAddressGov; }
        }

        /// <summary>
        /// 只跑排程的步驟
        /// </summary>
        public static List<string> OnlyStep
        {
            set
            {
                _onlyStep = "";
            }
            get
            {
                List<string> list = new List<string>();

                if (_onlyStep != "")
                    list = _onlyStep.Split(',').ToList();

                return list;
            }
        }

        #endregion
    }
}
