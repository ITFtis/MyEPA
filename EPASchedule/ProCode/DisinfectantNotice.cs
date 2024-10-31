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

                //所有藥劑
                var ants = DisinfectantService.GetAll();

                //City, Town, ContactUnit, DrugName
                var tmp = ants.Select(a => new
                {
                    City = a.City,
                    Town = a.Town,
                    ContactUnit = a.ContactUnit,
                    DrugName = a.DrugName,
                    ServiceLife = a.ServiceLife,
                    ServiceLifeDiffDay = DateFormat.ToDiffDays(a.ServiceLife, DateTime.Now),
                });

                //待警示藥劑(母體)
                var datas = tmp.Where(a => a.ServiceLifeDiffDay <= AppConfig.ValidDay).ToList();

                //待警示藥劑單位
                var units = datas.Select(a => new { a.City, a.Town }).Distinct().OrderBy(a => a.City);

                //鄉鎮帳號
                var accounts = new UsersService().GetAll().Where(a => a.MainContacter == "是").ToList();

                foreach (var unit in units)
                {
                    var infos = datas.Where(a => a.City == unit.City && a.Town == unit.Town);
                    var account = accounts.Where(a => a.City == unit.City && a.Town == unit.Town).FirstOrDefault();
                    
                    //紀錄查無主要聯絡人資訊
                    if (account == null)
                    {                        
                        var msgs = infos.Select((a, index) => (index + 1).ToString() + "." + "藥劑(" + a.DrugName + ")：效期(" + DateFormat.ToDate14(a.ServiceLife) + "),天數(" + a.ServiceLifeDiffDay + ")");
                        string msg = string.Join("\r\n", msgs);

                        string errors = string.Format("\r***查無此單位主要聯絡人：{0}{1}***\r{2}", 
                                                unit.City, unit.Town, 
                                                msg);
                        logger.Error(errors);
                        continue;
                    }
                    else
                    {
                        string aa = "!@3";
                    }
                    
                    
                }

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
