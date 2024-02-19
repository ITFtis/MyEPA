using MyEPA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models.BaseModels
{
    public class ApplyBaseStatusCountModel
    {
        public int CityId { get; set; }
        public int TownId { get; set; }
        public ApplyStatusEnum EPAConfirmStatus { get; set; }
        public ApplyStatusEnum EPBConfirmStatus { get; set; }
        public decimal Count { get; set; }

        public bool IsToEpa { get; set; }
    }

    public class ApplyBaseEPBStatusCountModel
    {
        public int CityId { get; set; }
        public int TownId { get; set; }
        public ApplyStatusEnum EPBConfirmStatus { get; set; }
        public int Count { get; set; }
    }
}