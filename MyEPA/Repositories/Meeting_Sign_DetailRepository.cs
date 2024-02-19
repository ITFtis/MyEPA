using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Repositories
{
    public class Meeting_Sign_DetailRepository : BaseEMISRepository<Meeting_Sign_DetailModel>
    {
        public List<Meeting_Sign_DetailModel> GetListByMeetingId(int meetingid)
        {
            string whereSql = @"
Where Meeting_ID = @meetingid
";
            return GetListByWhereSQL(whereSql, new { meetingid });
        }

        public Dictionary<int,int> GetMeeting_SignAttendance()
        {
            string sql = @"
SELECT Meeting_ID, Count(1) Count
FROM Meeting_Sign_Detail
GROUP BY Meeting_ID";

            return GetListBySQL<dynamic>(sql).Select(e => new
            {
                Meeting_ID = e.Meeting_ID,
                Count = e.Count
            }).ToDictionary(e => (int)e.Meeting_ID, e => (int)e.Count);
        }
    }
}