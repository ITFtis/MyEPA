using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPASchedule
{
    internal class Program
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            DateTime start_time = DateTime.Now;

            //1.消毒藥劑到期通知
            if (AppConfig.OnlyStep.IndexOf("消毒藥劑到期通知") > -1)
            {
                //呼叫這個網站的隨便一個網頁，使得IIS在重新啟動它，這樣Global又會跑了
                start_time = DateTime.Now;
                logger.Info("消毒藥劑到期通知" + ": starting...");
                DisinfectantNotice DalDisinfectantNotice = new DisinfectantNotice();
                DalDisinfectantNotice.Execute();
                logger.Info(@"Execution time(sec)=" + DateTime.Now.Subtract(start_time).TotalSeconds);
                logger.Info(typeof(Program).FullName + ": done....");
                logger.Info("");
            }

            //2.數量低於閥值到期通知
            if (AppConfig.OnlyStep.IndexOf("數量低於閥值到期通知") > -1)
            {
                //呼叫這個網站的隨便一個網頁，使得IIS在重新啟動它，這樣Global又會跑了
                start_time = DateTime.Now;
                logger.Info("數量低於閥值到期通知" + ": starting...");
                CtPointNotice DalCtPointNotice = new CtPointNotice();
                DalCtPointNotice.Execute();
                logger.Info(@"Execution time(sec)=" + DateTime.Now.Subtract(start_time).TotalSeconds);
                logger.Info(typeof(Program).FullName + ": done....");
                logger.Info("");
            }
        }
    }
}
