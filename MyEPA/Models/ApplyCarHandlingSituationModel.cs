namespace MyEPA.Models
{
    public class ApplyCarHandlingSituationModel
    {
        [AutoKey]
        public int Id { get; set; }
        [Foreignkey]
        public int ApplyId { get; set; }

        public int Type { get; set; }

        public int CarType { get; set; }

        public int Quantity { get; set; }

        public int Day { get; set; }
        public decimal Subsidy { get; set; }
    }
}