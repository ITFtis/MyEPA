using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEPA.Models;
using MyEPA.Services;

namespace EPASchedule
{
    internal class DisinfectantNotice
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private EsdmsModelContextExt _dbContextEsdms = new EsdmsModelContextExt();
        //private MisModelContext _dbContextMis = new MisModelContext();

        public void Execute()
        {
            try
            {
                if (!Do())
                {
                    logger.Error("執行失敗");
                }
                else
                {
                    logger.Info("執行成功");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }

        private bool Do()
        {
            try
            {
                DisinfectantService DisinfectantService = new DisinfectantService();

                //母體資料
                var datas = DisinfectantService.GetAll();

                //City, Town, ContactUnit, DrugName
                var tmp = datas.Select(a => new
                {
                    City = a.City,
                    Town = a.Town,
                    ContactUnit = a.ContactUnit,
                    DrugName = a.DrugName,
                    ServiceLife = a.ServiceLife,
                    ServiceLifeDiffDay = DateFormat.ToDiffDays(a.ServiceLife, DateTime.Now),
                });

                var notices = tmp.Where(a => a.ServiceLifeDiffDay <= AppConfig.ValidDay).ToList();

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("執行錯誤 - Do");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return true;
        }
    }
}
