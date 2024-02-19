using MyEPA.Enums;

namespace MyEPA.Models.QueryModel
{
    public class WaterCheckStatisticsQueryModel
    {
        public int CityId { get; set; }
        public WaterCheckTypeEnum Type { get; set; }
        public WaterCheckDetailStatusEnum Status { get; set; }
        public int Count { get; set; }
    }
}