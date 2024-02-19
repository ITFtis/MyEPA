using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Repositories
{
    public class FileRepository : BaseEMISRepository<FileDataModel>
    {
        public bool IsExistsByTypeAndSourceId(SourceTypeEnum sourceType, int sourceId)
        {
            string whereSql = "Where SourceType = @SourceType AND SourceId = @SourceId";
            return IsExistsByWhereSQL(whereSql, new { sourceType, sourceId });
        }
        public List<FileDataModel> GetBySource(SourceTypeEnum sourceType, int sourceId)
        {
            string whereSql = "Where SourceType = @SourceType AND SourceId = @SourceId";

            Dictionary<string, object> param = new Dictionary<string, object> { };
            param.Add("SourceType", sourceType.ToInteger());
            param.Add("SourceId", sourceId);

            return GetListByWhereSQL(whereSql,param);
        }

        public List<FileDataModel> GetListBySource(SourceTypeEnum sourceType)
        {
            string whereSql = "Where SourceType = @SourceType";

            Dictionary<string, object> param = new Dictionary<string, object> { };
            param.Add("SourceType", sourceType);

            return GetListByWhereSQL(whereSql, param);
        }
    }
}