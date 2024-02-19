using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class TeamShiftScheduleUserRepository : BaseEMISRepository<TeamShiftScheduleUserModel>
    {
        public List<TeamShiftScheduleUserModel> GetCheckDataByFilter(ShiftScheduleCheckDataFilterParameterModel filter)
        {
            string sql = @"
SELECT TSSU.*
FROM TeamShiftSchedule TSS
JOIN TeamShiftScheduleUser TSSU ON TSS.Id = TSSU.TeamShiftScheduleId
WHERE 
	DATEADD (HOUR , -@Hour , TSS.StartTime ) <= @Time 
	AND DATEADD (HOUR , @Hour ,TSS.EndTime ) >= @Time 
	AND TSSU.UserId = @UserId";

            return GetListBySQL<TeamShiftScheduleUserModel>(sql, filter);
        }
        public List<TeamShiftScheduleUserModel> GetByShiftScheduleId(int teamScheduleId)
        {
            string sql = @"
SELECT *
FROM [dbo].[TeamShiftScheduleUser]
WHERE TeamShiftScheduleId = @TeamShiftScheduleId";

            return GetListBySQL<TeamShiftScheduleUserModel>(sql, new { TeamShiftScheduleId = teamScheduleId });
        }
        public List<TeamShiftScheduleUserJoinUsersModel> GetByTeamShiftScheduleIds(IEnumerable<int> teamShiftScheduleIds)
        {
            string sql = @"
SELECT SSU.*,U.MobilePhone,U.Name
FROM [dbo].[TeamShiftScheduleUser] AS SSU
JOIN Users AS U ON U.Id = SSU.UserId
WHERE SSU.TeamShiftScheduleId IN @TeamShiftScheduleIds";

            return GetListBySQL<TeamShiftScheduleUserJoinUsersModel>(sql, new { TeamShiftScheduleIds = teamShiftScheduleIds });
        }

        public void DeleteByShiftScheduleId(int teamScheduleId)
        {
            DeleteByShiftScheduleIds(teamScheduleId.ToListCollection());
        }
        public void DeleteByShiftScheduleIds(List<int> teamScheduleIds)
        {
            string sql = @"
DELETE
FROM TeamShiftScheduleUser
WHERE [TeamShiftScheduleId] IN @TeamShiftScheduleIds";
            ExecuteSQL(sql, new { TeamShiftScheduleIds = teamScheduleIds });
        }
    }
}