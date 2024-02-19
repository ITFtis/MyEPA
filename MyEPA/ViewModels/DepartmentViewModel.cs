using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class DepartmentViewModel
    {
        [DisplayName("ID")]
        public int? Id { get; set; }
        [DisplayName("單位名稱")]
        public string Name { get; set; }
    }
}