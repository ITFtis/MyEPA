using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.ViewModels
{
    public class ApplySupportSubsidyReportViewModel
    {
        [DisplayName("環境部 Epa")]
        public List<ApplySupportSubsidyReportDetailViewModel> EPACountingReport { get; } = new List<ApplySupportSubsidyReportDetailViewModel>();
        [DisplayName("環保局 Epb")]
        public List<ApplySupportSubsidyReportDetailViewModel> EPBCountingReport { get; } = new List<ApplySupportSubsidyReportDetailViewModel>();
    }
}