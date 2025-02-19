using MyEPA.EPA.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MyEPA.Models
{
    public class ResourcesReportCityModel : ResourcesReportBaseModel
    {
        [DisplayName("縣市名稱")]
        public string City { get; set; }
        [DisplayName("縣市")]
        public int CityId { get; set; }
    }
    public class ResourcesReportTownModel : ResourcesReportBaseModel
    {
        [DisplayName("區")]
        public string Town { get; set; }
        public int TownId { get; set; }
        [DisplayName("縣市名稱")]
        public string City { get; set; }
        public int CityId { get; set; }
    }
    public class ResourcesReportBaseModel
    {
        [DisplayName("人員組織(人)")]
        [DataTableSum]
        public int UserCount { get; set; }
        [DataTableSum]
        [DisplayName("車輛設備(台)")]
        public int VehicleCount { get; set; }
        [DataTableSum]
        [DisplayName("消毒設備(台)")]
        public decimal DisinfectorCount { get; set; }
        [DataTableSum]
        [DisplayName("消毒藥品(固體)(公斤)")]
        public decimal DisinfectantSolidAmount { get; set; }
        [DataTableSum]
        [DisplayName("消毒藥品(液體含乳劑與油劑)(公升")]
        public decimal DisinfectantLiquidAmount { get; set; }
        [DataTableSum]
        [DisplayName("病媒防治業(家)")]
        public int PestCount { get; set; }
        [DataTableSum]
        [DisplayName("臨時廢棄物置物場(處)")]
        public int DumpCount { get; set; }
        [DataTableSum]
        [DisplayName("流動廁所(處)")]
        public int ToiletCount { get; set; }
        [DataTableSum]
        [DisplayName("民間志工(人)")]
        public int VolunteerCount { get; set; }
        [DisplayName("通報負責人姓名")]
        public string OwnerName { get; set; }
        [DisplayName("通報負責人電話")]
        public string OwnerMobilePhone { get; set; }
        public DateTime? VehicleConfirmTime { get; set; }

        public DateTime? VehicleUpdateTime { get; set; }
        public DateTime? UsersConfirmTime { get; set; }



        
        public DateTime? UsersUpdateTime { get; set; }
        
        public DateTime? DisinfectorConfirmTime { get; set; }
        
        public DateTime? DisinfectorUpdateTime { get; set; }
        
        public DateTime? DisinfectantConfirmTime { get; set; }
        
        public DateTime? DisinfectantUpdateTime { get; set; }
        
        public DateTime? PestConfirmTime { get; set; }
        
        public DateTime? PestUpdateTime { get; set; }
        
        public DateTime? DumpConfirmTime { get; set; }
        
        public DateTime? DumpUpdateTime { get; set; }
        
        public DateTime? ToiletConfirmTime { get; set; }
        
        public DateTime? ToiletUpdateTime { get; set; }
        
        public DateTime? VolunteerConfirmTime { get; set; }
        
        public DateTime? VolunteerUpdateTime { get; set; }
        [DisplayName("最後更新時間")]
        public DateTime? UpdateTime
        {
            get
            {
                DateTime? result = null;
                List<DateTime?> times = new List<DateTime?>();
                times.Add(VehicleUpdateTime);
                times.Add(UsersUpdateTime);
                times.Add(DisinfectorUpdateTime);
                times.Add(DisinfectantUpdateTime);
                times.Add(PestUpdateTime);
                times.Add(DumpUpdateTime);
                times.Add(ToiletUpdateTime);
                times.Add(VolunteerUpdateTime);
                return times.Where(e => e.HasValue).Max(e => e);
            }
        }
        [DisplayName("最後確認時間")]
        public DateTime? ConfirmTime
        {
            get
            {
                DateTime? result = null;
                List<DateTime?> times = new List<DateTime?>();
                times.Add(VehicleConfirmTime);
                times.Add(UsersConfirmTime);
                times.Add(DisinfectorConfirmTime);
                times.Add(DisinfectantConfirmTime);
                times.Add(PestConfirmTime);
                times.Add(DumpConfirmTime);
                times.Add(ToiletConfirmTime);
                times.Add(VolunteerConfirmTime);
                return times.Where(e => e.HasValue).Max(e => e);
            }
        }
    }
}