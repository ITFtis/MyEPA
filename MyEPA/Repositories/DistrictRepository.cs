using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class DistrictRepository : BaseEMISRepository<DistrictModel>
    {
        public DistrictModel GetByDistrictName(string districtName)
        {
            string whereSQL = "Where DistrictName = @districtName";
            return GetByWhereSQL(whereSQL, new { districtName });
        }
    }
}