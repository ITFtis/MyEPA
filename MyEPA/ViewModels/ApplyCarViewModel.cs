using MyEPA.Models;

namespace MyEPA.ViewModels
{
    public class ApplyCarViewModel : ApplyCarModel
    {
        public FileDataModel FileData { get; set; }

        public string CityName { get; set; }

        public string TownName { get; set; }

        /// <summary>
        /// 建立者單位
        /// </summary>
        public string CreateUserDuty { get; set; }
    }
}