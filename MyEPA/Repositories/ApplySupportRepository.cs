using MyEPA.Repositories.BaseRepositories;

namespace MyEPA.Repositories
{
    public class ApplySupportRepository : BaseEMISRepository
    {
        public decimal GetAllPrice(int diasterId)
        {
            string sql = @"
SELECT ISNULL(SUM(Subsidy),0)
FROM
(
	SELECT ap.DiasterId,ahs.Subsidy,type
	FROM ApplyPeople ap
	JOIN [ApplyHandlingSituation] ahs on ap.Id = ahs.ApplyId AND ahs.ApplyType = 3
	UNION
	SELECT als.DiasterId,ahs.Subsidy,type
	FROM ApplySubsidy als
	JOIN [ApplyHandlingSituation] ahs on als.Id = ahs.ApplyId AND ahs.ApplyType = 5
	UNION
	SELECT ao.DiasterId,ahs.Subsidy,type
	FROM ApplyOther ao
	JOIN [ApplyHandlingSituation] ahs on ao.Id = ahs.ApplyId AND ahs.ApplyType = 5
	UNION
	SELECT ac.DiasterId,achs.Subsidy,type
	FROM ApplyCar ac
	JOIN ApplyCarHandlingSituation achs on ac.Id = achs.ApplyId
	UNION
	SELECT am.DiasterId,achs.Subsidy,type
	FROM ApplyMedicine am
	JOIN ApplyMedicineHandlingSituation achs on am.Id = achs.ApplyId
	UNION
	SELECT ade.DiasterId,adehs.Subsidy,type
	FROM ApplyDisinfectionEquipment ade
	JOIN ApplyDisinfectionEquipmentHandlingSituation adehs on ade.Id = adehs.ApplyId
) AS Apply
WHERE Apply.DiasterId = @DiasterId and type = 2";
            return GetScalarBySQLScript<int>(sql, new { DiasterId = diasterId });
        }
    }
}