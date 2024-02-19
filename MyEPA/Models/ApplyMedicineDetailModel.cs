using MyEPA.Enums;

namespace MyEPA.Models
{
    public class ApplyMedicineDetailModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int ApplyMedicineId { get; set; }

        public ApplyMedicineTypeEnum MedicineType { get; set; }

        public int Quantity { get; set; }
    }
}