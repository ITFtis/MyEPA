using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class ContactManualRepository : BaseEMISBaseModelRepository<ContactManualModel>
    {
        public List<ContactManualModel> GetListByType(ContactManualTypeEnum type)
        {
            return GetListByFilter(new ContactManualFilterParameter 
            {
                Types = type.ToListCollection()
            });
        }
        public List<ContactManualModel> GetListByFilter(ContactManualFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }
        private static string GetWhereSQLByFilter(ContactManualFilterParameter filter)
        {
            string whereSQL = "where 1=1";

            if (filter.Types.IsNotEmpty())
            {
                whereSQL += " AND Type IN @Types";
            }

            if (filter.SourceIds.IsNotEmpty())
            {
                whereSQL += " AND SourceId IN @SourceIds";
            }

            return whereSQL;
        }
    }
}