﻿using DocumentFormat.OpenXml.Office2010.Excel;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Services
{
    public class LogDisinfectorService
    {
        //初始閥值(對應災害編號)
        public static int iniDiasterId = 999999999;

        LogDisinfectorRepository LogDisinfectorRepository = new LogDisinfectorRepository();
        DiasterRepository DiasterRepository = new DiasterRepository();

        public List<LogDisinfectorModel> GetByDiasterId(int diasterId)
        {
            LogDisinfectorFilterParameter filter =
                new LogDisinfectorFilterParameter
                {
                    DiasterIds = diasterId.ToListCollection(),
                };

            var logDisinfector = LogDisinfectorRepository
                .GetByFilter(filter);
           
            return logDisinfector;
        }

        /// <summary>
        /// 閥值資料建置
        /// </summary>
        /// <param name="user"></param>
        /// <param name="diasterId">災害id</param>
        public void Create(UserBriefModel user, int diasterId)
        {
            DisinfectorService DisinfectorService = new DisinfectorService();
            LogDisinfectorRepository LogDisinfectorRepository = new LogDisinfectorRepository();

            var datas = DisinfectorService.GetAll();

            DateTime date = DateTime.Now;
            foreach (var data in datas)
            {
                //消毒設備
                LogDisinfectorModel logFector = new LogDisinfectorModel();
                ClassUtility.CopyPropertiesTo(data, logFector);
                
                logFector.DiasterId = diasterId;
                logFector.CtPoint = float.Parse(logFector.Amount) / 2;
                logFector.LogBDate = date;
                logFector.LogBUser = user.UserName;

                LogDisinfectorRepository.Create(logFector);
            }
        }

        //利用災害Id刪除
        public void DeleteByDiasterId(int DiasterId)
        {
            LogDisinfectorRepository.Delete(DiasterId);
        }
    }
}