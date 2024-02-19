using MyEPA.Enums;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class ContactManualDepartmentModel : BaseModel
    {
        [AutoKey]
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("單位名稱")]
        public string Name { get; set; }
        public ContactManualDepartmentTypeEnum Type { get; set; }
        public int Sort { get; set; }
    }
}