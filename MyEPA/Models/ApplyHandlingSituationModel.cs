using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class ApplyHandlingSituationModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int ApplyType { get; set; }

        public int ApplyId { get; set; }

        public int Type { get; set; }

        public string Description { get; set; }

        public decimal Subsidy { get; set; }

    }
}