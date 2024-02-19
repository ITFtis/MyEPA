using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.ViewModels
{
    public class ApplySupportCheckCountingViewModel
    {
        public int TownId { get; set; }
        
        public string TownName { get; set; }

        public int Pending { get; set; }

        public int Processing { get; set; }

        public int SendToEpa { get; set; }

        public int EpbConfrim { get; set; }

        public int Reject { get; set; }
    }
}