using MyEPA.Enums;
using System;

namespace MyEPA.Models
{
    public class DeliveryStatusResultModel
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public DateTime? SendTime { get; set; }
        public decimal? Cost { get; set; }
        public SendTextLogDetailStatusEnum Status { get; set; }
    }
}