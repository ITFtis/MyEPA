namespace MyEPA.Models
{

    public class ApplyPeopleHandlingSituationModel
    {
        [AutoKey]
        public int Id { get; set; }
        [Foreignkey]
        public int ApplyId { get; set; }

        public int Type { get; set; }

        public int PeopleType { get; set; }

        public int Quantity { get; set; }

        public int Day { get; set; }
        public decimal Subsidy { get; set; }
    }
}