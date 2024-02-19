using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;

namespace MyEPA.Repositories
{
    public class DutyRepository: BaseEMISRepository<DutyModel>
    {
        public DutyModel GetByDutyName(string duty)
        {
            string whereSQL = "Where Name=@name";
            return GetByWhereSQL(whereSQL, new { Name = duty });
        }
    }
}