namespace MyEPA.Models
{

    public class ApplyMedicineHandlingSituationModel
    {
        [AutoKey]
        public int Id { get; set; }
        [Foreignkey]
        public int ApplyId { get; set; }

        public int Type { get; set; }

        public int MedicineType { get; set; }

        public int Quantity { get; set; }
        public decimal Subsidy { get; set; }
    }
}