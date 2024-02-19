using MyEPA.Enums;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class ApplyStatusReportModel
    {
        public int CityId { get; set; }
        public ApplyStatusEnum EPAConfirmStatus { get; set; }
        public int EPAConfirmStatusCount { get; set; }
        public ApplyStatusEnum EPBConfirmStatus { get; set; }
        public int EPBConfirmStatusCount { get; set; }
    }
    public class ApplyReportRepository : BaseEMISRepository
    {
        public List<ApplyStatusReportModel> GetApplyStatusReport(int diasterId)
        {
            string sql = @"
SELECT 
	CityId
	,EPAConfirmStatus
	,COUNT(EPAConfirmStatus)EPAConfirmStatusCount
	,EPBConfirmStatus
	,COUNT(EPBConfirmStatus)EPBConfirmStatusCount
FROM
(
	SELECT ap.DiasterId,ap.EPAConfirmStatus,ap.EPBConfirmStatus,ap.CityId
	FROM ApplyPeople ap
	UNION
	SELECT als.DiasterId,als.EPAConfirmStatus,als.EPBConfirmStatus,als.CityId
	FROM ApplySubsidy als
	UNION
	SELECT ao.DiasterId,ao.EPAConfirmStatus,ao.EPBConfirmStatus,ao.CityId
	FROM ApplyOther ao
	UNION
	SELECT ac.DiasterId,ac.EPAConfirmStatus,ac.EPBConfirmStatus,ac.CityId
	FROM ApplyCar ac
	UNION
	SELECT am.DiasterId,am.EPAConfirmStatus,am.EPBConfirmStatus,am.CityId
	FROM ApplyMedicine am
	UNION
	SELECT ade.DiasterId,ade.EPAConfirmStatus,ade.EPBConfirmStatus,ade.CityId
	FROM ApplyDisinfectionEquipment ade
	JOIN ApplyDisinfectionEquipmentHandlingSituation adehs on ade.Id = adehs.ApplyId
) AS T
WHERE T.CityId IS NOT NULL AND T.EPAConfirmStatus > 0
AND T.DiasterId = @DiasterId
GROUP BY EPAConfirmStatus,EPBConfirmStatus,CityId
ORDER BY T.CityId
";
			return GetListBySQL<ApplyStatusReportModel>(sql, new { diasterId });
        }

    }
}