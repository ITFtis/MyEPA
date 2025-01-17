﻿namespace MyEPA.Models
{
    public class ApplyDisinfectionEquipmentHandlingSituationModel
    {
        [AutoKey]
        public int Id { get; set; }
        [Foreignkey]
        public int ApplyId { get; set; }

        public int Type { get; set; }

        public string Name { get; set; }

        public int Day { get; set; }
        public decimal Subsidy { get; set; }
    }
}