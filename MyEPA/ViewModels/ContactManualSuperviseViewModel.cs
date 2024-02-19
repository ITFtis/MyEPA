using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class ContactManualSuperviseViewModel
    {
        public int? Id { get; set; }
        [DisplayName("單位")]
        public string DepartmentName { get; set; }
        [DisplayName("單位")]
        public int DepartmentId { get; set; }
        [DisplayName("督導業務")]

        public string Describe { get; set; }
    }
}