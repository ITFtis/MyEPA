using MyEPA.Enums;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class ContactManualRoleModel : BaseModel
    {
        [AutoKey]
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("職務名稱")]
        public string Name { get; set; }
        public ContactManualRoleTypeEnum Type { get; set; }
    }
}