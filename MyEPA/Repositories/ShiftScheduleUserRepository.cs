using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class MainShiftScheduleUserRepository : BaseEMISRepository<MainShiftScheduleUserModel>
    {
        public List<MainShiftScheduleUserModel> GetCheckDataByFilter(ShiftScheduleCheckDataFilterParameterModel filter)
        {

            string sql = @"
SELECT SSU.*
FROM [dbo].[MainShiftSchedule] SS
JOIN MainShiftScheduleUser SSU ON SS.Id = SSU.MainShiftScheduleId
WHERE 
	SS.Date = convert(char, @Time, 112) 
	AND SSU.UserId = @UserId";

            return GetListBySQL<MainShiftScheduleUserModel>(sql, filter);
        }
        public List<MainShiftScheduleUserJoinUsersModel> GetByShiftScheduleIds(IEnumerable<int> shiftScheduleIds)
        {
            string sql = @"
SELECT SSU.*,U.MobilePhone,U.Name
FROM [dbo].[MainShiftScheduleUser] AS SSU
LEFT JOIN Users AS U ON U.Id = SSU.UserId
WHERE SSU.MainShiftScheduleId IN @ShiftScheduleIds";

            return GetListBySQL<MainShiftScheduleUserJoinUsersModel>(sql, new { ShiftScheduleIds = shiftScheduleIds });
        }

        public void DeleteByMainShiftScheduleIds(List<int> mainScheduleIds)
        {
            string sql = @"
DELETE
FROM MainShiftScheduleUser
WHERE [MainShiftScheduleId] IN @MainScheduleIds";
            ExecuteSQL(sql, new { MainScheduleIds = mainScheduleIds });
        }

        public IEnumerable<MainShiftScheduleUserModel> GetByMainShiftScheduleId(int mainShiftScheduleId)
        {
            string sql = @"
SELECT *
FROM [dbo].[MainShiftScheduleUser]
WHERE MainShiftScheduleId = @mainShiftScheduleId";

            return GetListBySQL<MainShiftScheduleUserModel>(sql, new { MainShiftScheduleId = mainShiftScheduleId });
        }
    }
    public class ShiftScheduleUserRepository : BaseEMISRepository<ShiftScheduleUserModel>
    {
        public List<ShiftScheduleUserModel> GetCheckDataByFilter(ShiftScheduleCheckDataFilterParameterModel filter)
        {

            string sql = @"
SELECT SSU.*
FROM [dbo].[ShiftSchedule] SS
JOIN ShiftScheduleUser SSU ON SS.Id = SSU.ShiftScheduleId
WHERE 
	SS.Date = convert(char, @Time, 112) 
	AND SSU.UserId = @UserId";

            return GetListBySQL<ShiftScheduleUserModel>(sql, filter);
        }
        public List<ShiftScheduleUserModel> GetByShiftScheduleId(int shiftScheduleId)
        {
            string sql = @"
SELECT *
FROM [dbo].[ShiftScheduleUser]
WHERE ShiftScheduleId = @ShiftScheduleId";

            return GetListBySQL<ShiftScheduleUserModel>(sql, new { ShiftScheduleId = shiftScheduleId });
        }
        public List<ShiftScheduleUserJoinUsersModel> GetByShiftScheduleIds(IEnumerable<int> shiftScheduleIds)
        {
            string sql = @"
SELECT SSU.*,U.MobilePhone,U.Name
FROM [dbo].[ShiftScheduleUser] AS SSU
JOIN Users AS U ON U.Id = SSU.UserId
WHERE SSU.ShiftScheduleId IN @ShiftScheduleIds";

            return GetListBySQL<ShiftScheduleUserJoinUsersModel>(sql, new { ShiftScheduleIds = shiftScheduleIds });
        }

        public void DeleteByShiftScheduleId(int scheduleId)
        {
            DeleteByShiftScheduleIds(scheduleId.ToListCollection());
        }
        public void DeleteByShiftScheduleIds(List<int> scheduleIds)
        {
            string sql = @"
DELETE
FROM ShiftScheduleUser
WHERE [ShiftScheduleId] IN @ScheduleIds";
            ExecuteSQL(sql, new { ScheduleIds = scheduleIds });
        }
    }
}