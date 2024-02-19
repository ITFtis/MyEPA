namespace MyEPA.Models.QueryModel
{
    public class InfectiousDiseaseStatisticsModel
    {
        public int TownId { get; set; }
        public string TownName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int HomeQuarantineCount { get; set; }
        public decimal HomeQuarantineGarbageAmount { get; set; }
        public int HomeInspectionCount { get; set; }
        public decimal HomeInspectionGarbageAmount { get; set; }
        public int InspectionHotelCount { get; set; }
        public decimal InspectionHotelGarbageAmount { get; set; }
        public int MaskCheckTimes { get; set; }
        public int ReportTimes { get; set; }

    }
}