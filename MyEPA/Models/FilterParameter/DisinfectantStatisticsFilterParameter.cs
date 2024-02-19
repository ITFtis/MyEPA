using MyEPA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models.FilterParameter
{
    public class DisinfectantStatisticsFilterParameter
    {
        public IEnumerable<int> CityIds { get; set; }
        public DateTime? ServiceLifeStartTime { get; set; }
        public DateTime? ServiceLifeEndTime { get; set; }
    }
    public class DisinfectantReportFilterParameter
    {
        public string City { get; set; }
        public string Town { get; set; }
        public DisinfectantUseTypeEnum? UseType { get; set; }
        public string DrugName { get; set; }
        public string DrugType { get; set; }

        public string ActiveIngredients1 { get; set; }

        public string ActiveIngredients2 { get; set; }
        public string DrugState { get; set; }
        

        public DateTime? ServiceLifeStartTime { get; set; }
        public DateTime? ServiceLifeEndTime { get; set; }
    }
}