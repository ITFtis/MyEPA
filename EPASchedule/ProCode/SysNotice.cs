using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPASchedule
{
    internal class SysNotice
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute()
        {
            try
            {
                //1.開口合約通知
                logger.Info("1.開口合約通知");
                if (!DoOpenContract())
                {
                    logger.Error("執行失敗");
                }
                else
                {
                    logger.Info("執行成功");
                }

                //2.xxxxxxxxxxx
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }

        private bool DoOpenContract()
        {
            try
            {
                logger.Info("aaaabcd");
            }
            catch (Exception ex)
            {
                logger.Error("執行錯誤 - Do");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);

                return false;
            }

            return true;
        }
    }
}
