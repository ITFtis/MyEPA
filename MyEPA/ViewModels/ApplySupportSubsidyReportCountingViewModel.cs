using MyEPA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.ViewModels
{
    public class ApplySupportSubsidyReportCountingViewModel
    {
        public string ApplyType { get; set; }
        public ApplyStatusEnum? ConfirmStatus { get; set; }
        public int Count { get; set; }
    }
}