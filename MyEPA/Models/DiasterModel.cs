using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class DiasterModel
    {
        [AutoKey]
        public int Id { get; set; }

        public string DiasterName { get; set; }

        public string DiasterType { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Comment { get; set; }

        public string DiasterState { get; set; }

        public string CoverCity { get; set; }

        public int Status { get; set; }

    }
}