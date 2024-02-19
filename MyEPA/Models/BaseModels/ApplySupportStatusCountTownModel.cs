namespace MyEPA.Models.BaseModels
{
    public class ApplySupportStatusCountTownModel : ApplySupportStatusCountCityModel
    {
        public int TownId { get; set; }
    }
    public class ApplySupportStatusCountCityModel
    {
        public int CityId { get; set; }
        public int Count { get; set; }
        public int EpaConfrimCount { get; set; }
        public int EpbConfrimCount { get; set; }
    }
}