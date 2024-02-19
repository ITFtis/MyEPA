using MyEPA.Models;
using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class ContactManualSupervisionFileDataViewModel : FileDataModel 
    {
        [DisplayName("章節")]
        public string Chapter { get; set; }
    }
    public class ContactManualFileDataViewModel : FileDataModel
    {
        [DisplayName("部門")]
        public string DepartmentName { get; set; }
    }
}