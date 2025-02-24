﻿using MyEPA.Extensions;
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
        //初始閾值(對應災害編號)
        public const int iniDiasterId = 999999999;

        LogDisinfectantRepository LogDisinfectantRepository = new LogDisinfectantRepository();
        DiasterRepository DiasterRepository = new DiasterRepository();
        DisinfectantRepository DisinfectantRepository = new DisinfectantRepository();

        /// <summary>
        /// (閾值)正確災害編號，舊資料使用預設
        /// </summary>
        /// <param name="diasterId"></param>
        /// <returns></returns>
        public int GetYDiasterId(int diasterId)
        {
            int result = diasterId;

            LogDisinfectantFilterParameter filter = new LogDisinfectantFilterParameter
            {
                DiasterIds = diasterId.ToListCollection(),
            };

            List<LogDisinfectantModel> list = LogDisinfectantRepository.GetByFilter(filter);

            result = list.Count != 0 ? result : iniDiasterId;

            return result;
        }

        /// <summary>
        /// (閾值)消毒藥品，當下數量比較
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<LogDisinfectantViewModel> GetLogDisinfectantCurrentByFilter(LogDisinfectantFilterParameter filter)
        {
            var model = LogDisinfectantRepository.GetLogDisinfectantCurrentByFilter(filter);

            //個別現有數量明細
            if (filter.DiasterIds.Count > 0)
            {
                var curs = DisinfectantRepository.GetList();

                List<LogDisinfectantViewModel> removeList = new List<LogDisinfectantViewModel>();
                foreach (var item in model)
                {
                    var tmp = curs.Where(a => a.City == item.City && a.Town == item.Town
                                        && a.ContactUnit == item.ContactUnit
                                        && a.DrugName == item.DrugName);

                    var vs = tmp.Select(a => new { 
                                    Amount = a.Amount,
                                    a.ServiceLife,
                                    ServiceLifeDiffDay = DateFormat.ToDiffDays(a.ServiceLife, DateTime.Now),
                            });

                    if (filter.ServiceLifeDay.HasValue)
                    {
                        vs = vs.Where(a => a.ServiceLifeDiffDay <= filter.ServiceLifeDay.Value);

                        //主表不顯示：有效天數條件，若沒符合詳細資料
                        if (vs.Count() == 0)
                        {
                            removeList.Add(item);
                            continue;
                        }

                    }

                    var fs = vs.Select(a => new
                    {
                        Amount = a.Amount,
                        ServiceLifeName = a.ServiceLife != null ? DateFormat.ToDate4((DateTime)a.ServiceLife) : "",
                        ServiceLifeDay = a.ServiceLife != null ?
                                            (a.ServiceLifeDiffDay >= 0 ? a.ServiceLifeDiffDay.ToString() : "<span style='color:red'>" + a.ServiceLifeDiffDay.ToString() + "</span>")
                                            + "天"
                                        : "",
                    });

                    item.CurYearDesc = string.Join("<br />", fs.Select(a => "數量(" + a.Amount + ")" + "：效期(" + a.ServiceLifeName + ") " + a.ServiceLifeDay));
                }

                model = model.Where(a => !removeList.Any(b => a.SerialNo == b.SerialNo)).ToList();
            }

            return model;
        }


        /// <summary>
        /// 閾值資料建置
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