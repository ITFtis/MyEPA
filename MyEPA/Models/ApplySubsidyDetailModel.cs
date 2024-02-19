using MyEPA.Enums;

namespace MyEPA.Models
{
    public class ApplySubsidyDetailModel
    {
        public int Id { get; set; }

        public int ApplySubsidyId { get; set; }

        public int ApplySubsidyTypeId
        {
            get
            {
                return SubsidyType.ToInteger();
            }

            set 
            {
                SubsidyType = (ApplySubsidyTypeEnum)value;
            }
        }
        public ApplySubsidyTypeEnum SubsidyType { get; set; }
        public int ApplySubsidyTypeDetailId { get; set; }
        public int Quantity { get; set; }
        public int NeedDays { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
    }
}