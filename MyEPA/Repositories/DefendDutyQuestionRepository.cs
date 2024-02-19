using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Repositories
{
    public class DefendQuestionRepository : BaseEMISRepository<DefendQuestionModel>
    {
        public List<DefendDutyQuestionJoinModel> GetByDefendId(int dutyId, int defendId = 0)
        {
            string sql = @"
SELECT DQ.Id,DQ.DefendId,Q.Id QuestionId,Q.Title,Q.Note,DQ.Ans,DQ.Remark,DDQ.Sort
FROM DefendDutyQuestion DDQ
JOIN Question Q ON DDQ.QuestionId = Q.Id
LEFT JOIN DefendQuestion DQ ON DQ.QuestionId = Q.Id AND  DQ.DefendId = @DefendId
WHERE DDQ.DutyId = @DutyId ";
            return GetListBySQL<DefendDutyQuestionJoinModel>(sql, new
            {
                DutyId = dutyId,
                DefendId = defendId
            });
        }
        public void DeleteByDefendId(int defendId)
        {
            string whereSql = @"WHERE DefendId = @defendId";
            DeleteByWhereSQL(whereSql, new { defendId });
        }


        public List<DefendDutyQuestionJoinModel> GetByDiasterId(int diasterId, int cityId)
        {
            string sql = @"
SELECT DQ.Id,DQ.DefendId,DQ.QuestionId,Q.Title,Q.Note,DQ.Ans,DQ.Remark,DDQ.Sort
FROM DefendQuestion DQ
JOIN Question Q ON DQ.QuestionId = Q.Id
JOIN Defend D ON D.Id = DQ.DefendId
JOIN DefendDutyQuestion DDQ ON DDQ.DutyId = D.DutyId AND DDQ.QuestionId = DQ.QuestionId
WHERE D.CityId = @CityId AND D.DiasterId = @DiasterId
";
            return GetListBySQL<DefendDutyQuestionJoinModel>(sql, new
            {
                CityId = cityId,
                DiasterId = diasterId
            });
        }

    }
}