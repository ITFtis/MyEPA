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

        //利用災害Id刪除
        public void DeleteByDiasterIds(int DiasterId)
        {
            LogDisinfectorRepository.Delete(DiasterId);
        }
    }
}