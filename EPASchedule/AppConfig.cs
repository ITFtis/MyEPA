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

        private static string _onlyStep;

        #endregion

        #region 建構子

        static AppConfig()
        {
            _onlyStep = ConfigurationManager.AppSettings["OnlyStep"].ToString();
        }

        #endregion

        #region 公用屬性        

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
