using MyEPA.Models;
using System;
using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class ToiletLocationViewModel : ToiletLocationModel
    {
        [DisplayName("放置地點說明")]
        public string AddressDisplay { get; set; }
        [DisplayName("管理維護單位")]
        public string ManagementTownDisplay { get; set; }
        [DisplayName("租約起迄日期")]
        public string DateDisplay { get; set; }
    }   
    public class ToiletLocationStatisticsViewModel : ToiletLocationStatisticsModel
    {
        [DisplayName("租約起迄日期")]
        public string DateDisplay { get; set; }
    }
    
}