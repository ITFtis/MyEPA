using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class ShiftScheduleRepository : BaseEMISRepository<ShiftScheduleModel>
    {
        public List<AllShiftScheduleQueryModel> QueryAllShiftSchedule(int diasterId , DateTime date,bool? isNight)
        {
            string sql = @";
SELECT T.IsNight,T.DepartmentId,T.Name,T.MobilePhone,T.DepartmentName
FROM
(
    SELECT SS.DiasterId,SS.Date, SS.IsNight,SS.DepartmentId,U.Name,U.MobilePhone,D.Name DepartmentName
    FROM [dbo].[ShiftSchedule] SS
    JOIN ShiftScheduleUser SSU ON SS.Id = SSU.ShiftScheduleId
    JOIN Users U ON SSU.UserId = U.Id
    JOIN Department D ON SS.DepartmentId = D.Id
    UNION
    SELECT MSS.DiasterId,MSS.Date, MSS.IsNight,3 DepartmentId,U.Name,U.MobilePhone,'中央應變中心' DepartmentName
    FROM MainShiftSchedule MSS
    JOIN MainShiftScheduleUser MSSU ON MSS.Id = MSSU.MainShiftScheduleId
    JOIN Users U ON MSSU.UserId = U.Id
) AS T
WHERE DiasterId = @DiasterId AND Date = @DATE AND (@IsNight Is NULL OR IsNight = @IsNight)
ORDER BY IsNight,DepartmentId
";
            return GetListBySQL<AllShiftScheduleQueryModel>(sql, new { diasterId = diasterId, date = date, IsNight = isNight });
        }
        /// <summary>
        /// 取得輪班次數
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<UserShiftScheduleCountModel> GetUserShiftScheduleCount(UserShiftScheduleCountFilterModel filter)
        {
            string sql = @"
SELECT
	D.Name DepartmentName,U.Name,S.Hour,S.Count
FROM
(
	SELECT DepartmentId,UserId,SUM(S.Hour) Hour,COUNT(S.UserId) Count
	FROM
	(
		SELECT S.DepartmentId,SU.UserId, ISNULL(DATEDIFF(MINUTE,SU.CheckinTime,SU.Checkout) / 60.0,0) Hour
		FROM [dbo].ShiftSchedule S
		JOIN [dbo].ShiftScheduleUser SU ON S.[Id] = SU.[ShiftScheduleId]
		WHERE S.Date >= @StartTime AND S.Date <= @EndTime AND S.DiasterId = @DiasterId
	) AS S
	GROUP BY DepartmentId,UserId

	UNION

	SELECT DepartmentId,UserId,SUM(S.Hour) Hour,COUNT(S.UserId) Count
	FROM
	(
		SELECT S.DepartmentId,SU.UserId, ISNULL(DATEDIFF(MINUTE,SU.CheckinTime,SU.Checkout) / 60.0,0) Hour
		FROM [dbo].[TeamShiftSchedule] S
		JOIN [dbo].[TeamShiftScheduleUser] SU ON S.[Id] = SU.TeamShiftScheduleId
		WHERE S.StartTime >= @StartTime AND S.EndTime <= @EndTime AND S.DiasterId = @DiasterId
	) AS S
	GROUP BY DepartmentId,UserId
) S
JOIN Users U ON S.UserId = U.Id
JOIN Department D ON D.Id = S.DepartmentId
";
            if(filter.DepartmentId.HasValue)
            {
                sql += " Where S.DepartmentId = @DepartmentId";
            }

            return GetListBySQL<UserShiftScheduleCountModel>(sql, filter);
        }
        /// <summary>
        /// 取得部門輪班表By DiasterId
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="diasterId"></param>
        /// <returns></returns>
        public List<ShiftScheduleModel> GetDepartmentShiftScheduleByDiasterId(int departmentId,int diasterId)
        {
            string whereSql = "Where DepartmentId=@DepartmentId AND diasterId = @DiasterId";
            return GetListByWhereSQL(whereSql, new { DepartmentId = departmentId, DiasterId = diasterId });
        }
        
    }
    public class TeamShiftScheduleRepository : BaseEMISRepository<TeamShiftScheduleModel>
    {
        public List<TeamShiftScheduleModel> GetDepartmentShiftScheduleByDiasterId(int departmentId, int diasterId)
        {
            string whereSql = "Where DepartmentId=@DepartmentId AND diasterId = @DiasterId";
            return GetListByWhereSQL(whereSql, new { DepartmentId = departmentId, DiasterId = diasterId });
        }

    }
}