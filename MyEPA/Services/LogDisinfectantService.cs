using MyEPA.Extensions;
using MyEPA.Models.FilterParameter;
using MyEPA.Models;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Services
{
    public class LogDisinfectantService
    {
        //初始閥值(對應災害編號)
        public static int iniDiasterId = 999999999;

        LogDisinfectantRepository LogDisinfectantRepository = new LogDisinfectantRepository();
        DiasterRepository DiasterRepository = new DiasterRepository();

        public List<LogDisinfectantModel> GetByDiasterId(int diasterId)
        {
            LogDisinfectantFilterParameter filter =
                new LogDisinfectantFilterParameter
                {
                    DiasterIds = diasterId.ToListCollection(),
                };

            var LogDisinfectant = LogDisinfectantRepository
                .GetByFilter(filter);

            return LogDisinfectant;
        }

        /// <summary>
        /// (閥值)消毒藥品，當下數量比較
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<LogDisinfectantViewModel> GetLogDisinfectantCurrentByFilter(LogDisinfectantFilterParameter filter)
        {
            var model = LogDisinfectantRepository.GetLogDisinfectantCurrentByFilter(filter);

            //個別資訊
            if (filter.DiasterIds.Count > 0)
            {
                var logs = GetByDiasterId(filter.DiasterIds.First());
                foreach (var item in model)
                {
                    var tmp = logs.Where(a => a.City == item.City && a.Town == item.Town
                                        && a.ContactUnit == item.ContactUnit
                                        && a.DrugName == item.DrugName);

                    var fs = tmp.Select(a => new { 
                                    Amount = a.Amount,
                                    ServiceLifeName = a.ServiceLife != null ? DateFormat.ToDate4((DateTime)a.ServiceLife) : ""
                            });

                    item.CurYearDesc = string.Join("<br />", fs.Select(a => "數量(" + a.Amount + ")" + "：使用年限(" + a.ServiceLifeName + ")"));
                }
            }

            return model;
        }


        /// <summary>
        /// 閥值資料建置
        /// </summary>
        /// <param name="user"></param>
        /// <param name="diasterId">災害id</param>
        public void Create(UserBriefModel user, int diasterId)
        {
            DisinfectantService DisinfectantService = new DisinfectantService();
            LogDisinfectantRepository LogDisinfectantRepository = new LogDisinfectantRepository();

            var datas = DisinfectantService.GetAll();

            DateTime date = DateTime.Now;
            foreach (var data in datas)
            {
                //消毒藥劑
                LogDisinfectantModel logFectant = new LogDisinfectantModel();
                ClassUtility.CopyPropertiesTo(data, logFectant);

                logFectant.DiasterId = diasterId;
                logFectant.CtPoint = ((float)logFectant.Amount) / 2;
                logFectant.LogBDate = date;
                logFectant.LogBUser = user.UserName;

                LogDisinfectantRepository.Create(logFectant);
            }
        }

        //利用災害Id刪除
        public void DeleteByDiasterId(int DiasterId)
        {
            LogDisinfectantRepository.Delete(DiasterId);
        }
    }
}