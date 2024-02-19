using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class MainShiftScheduleRepository : BaseEMISRepository<MainShiftScheduleModel>
    {
        /// <summary>
        /// 取得部門輪班表By DiasterId
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="diasterId"></param>
        /// <returns></returns>
        public List<MainShiftScheduleModel> GetShiftScheduleByDiasterId(int diasterId)
        {
            string whereSql = "Where diasterId = @DiasterId";
            return GetListByWhereSQL(whereSql, new { DiasterId = diasterId });
        }

        public List<MainShiftScheduleModel> GetDepartmentShiftScheduleByDiasterId(int diasterId)
        {
            string whereSql = "Where diasterId = @DiasterId";
            return GetListByWhereSQL(whereSql, new { DiasterId = diasterId });
        }
    }
}