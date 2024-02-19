using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class DistrictModel
    {
        [AutoKey]
        public string Id { get; set; }

        public string DistrictName { get; set; }

        public string CoverArea { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Mail { get; set; }

        public string Service { get; set; }

        public string Human { get; set; }

        public string OutHuman { get; set; }

        public string ReadyHuman { get; set; }

        public string CleanCapacity { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? ConfirmTime { get; set; }

    }
}