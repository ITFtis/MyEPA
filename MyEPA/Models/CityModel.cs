using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class CityModel
    {
        [AutoKey]
        public int Id { get; set; }
        public string City { get; set; }
        public int AreaId { get; set; }
        public bool IsCounty { get; set; }
        public int WaterDivisionId { get; set; }
        public int Sort { get; set; }
        public int Type { get; set; }
    }

    public class CityWaterDivisionJoinModel
    {
        public int CityId { get; set; }
        public string City { get; set; }
        public int WaterDivisionId { get; set; }
        public string WaterDivision { get; set; }
        public int Sort { get; set; }
    }
}