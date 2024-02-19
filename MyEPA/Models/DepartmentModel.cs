using System.ComponentModel;

namespace MyEPA.Models
{
    public class DepartmentModel
    {
        [AutoKey]
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("單位名稱")]
        public string Name { get; set; }
    }
}