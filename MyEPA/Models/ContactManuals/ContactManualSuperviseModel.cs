using System.ComponentModel;

namespace MyEPA.Models
{
    public class ContactManualSuperviseModel : BaseModel
    {
        [AutoKey]
        public int Id { get; set; }
        [DisplayName("單位")]
        public int DepartmentId { get; set; }
        [DisplayName("督導業務")]
        public string Describe { get; set; }

    }
}