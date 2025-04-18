﻿using MyEPA.Models;
using System.Collections.Generic;

namespace MyEPA.ViewModels
{
    public class ApplyMedicineViewModel : ApplyMedicineModel
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