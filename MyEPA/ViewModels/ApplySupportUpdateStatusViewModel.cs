using MyEPA.Models;
using System.Collections.Generic;

namespace MyEPA.ViewModels
{
    /// <summary>
    /// 接收更新環保局, 環境部審核用 request viewModel
    /// </summary>
    public class ApplySupportUpdateStatusViewModel
    {
        public int ApplyId { get; set; }
        public bool CheckConfirm { get; set; }
        public string TextConfirm { get; set; }
        public bool CheckSendToEPA { get; set; }
        public string TextSendToEPA { get; set; }
        public bool CheckReject { get; set; }
        public string TextReject { get; set; }
    }
    
    public class ApplyHandlingSituationViewModel
    {
        public int Type { get; set; }
        public string Description { get; set; }
        public decimal Subsidy { get; set; }
    }
}