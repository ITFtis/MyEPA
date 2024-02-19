using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class SystemConfigSettingRepository : BaseEMISRepository<SystemConfigSettingModel>
    {
        public SystemConfigSettingModel Get(SystemConfigSettingFunctionEnum function)
        {
            return Get((int)function);
        }
    }
}