using MyEPA.Enums;
using System.ComponentModel;

namespace MyEPA.ViewModels
{
    /// <summary>
    /// 計算報表用的 model
    /// </summary>
    public class ApplySupportSubsidyReportDetailViewModel
    {
        [DisplayName("內容")]
        public ApplyTypeEnum ApplyType {get;set; }
        [DisplayName("待審核")]
        public int Pending { get; set; }
        [DisplayName("審核中")]
        public int Processing { get; set; }
        [DisplayName("已核定")]
        public int Confrim { get; set; }
        [DisplayName("退回")]
        public int Reject { get; set; }
    }
}