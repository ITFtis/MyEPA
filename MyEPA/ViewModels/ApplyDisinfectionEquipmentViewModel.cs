using MyEPA.Models;

namespace MyEPA.ViewModels
{
    public class ApplyDisinfectionEquipmentViewModel : ApplyDisinfectionEquipmentModel
    {
        public FileDataModel FileData { get; set; }

        public string CityName { get; set; }

        public string TownName { get; set; }
    }
}