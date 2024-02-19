namespace MyEPA.Models
{
    public class TownModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int CityId { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// 是否是環保局
        /// </summary>
        public bool IsTown { get; set; }

    }
}