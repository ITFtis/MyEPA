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
        LogDisinfectorRepository LogDisinfectorRepository = new LogDisinfectorRepository();
        DiasterRepository DiasterRepository = new DiasterRepository();

        public List<LogDisinfectorModel> GetByDiasterId(int diasterId)
        {
            DiasterModel diaster = DiasterRepository.Get(diasterId);

            if (diaster == null)
            {
                return new List<LogDisinfectorModel>();
            }

            LogDisinfectorFilterParameter filter =
                new LogDisinfectorFilterParameter
                {
                    DiasterIds = diasterId.ToListCollection(),
                };

            var logDisinfector = LogDisinfectorRepository
                .GetByFilter(filter);

            return logDisinfector;
        }

        //利用災害Id刪除
        public void DeleteByDiasterIds(int DiasterId)
        {
            LogDisinfectorRepository.Delete(DiasterId);
        }
    }
}