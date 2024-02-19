using System;

namespace MyEPA.Models
{
    public class ResourcesConfirmUpdateTimeDataModel
    {
        public int CityId { get; set; }
        public string City { get; set; }
        public int TownId { get; set; }
        public string TownI { get; set; }
        public DateTime? VehicleUpdateTime { get; set; }
        public DateTime? VehicleConfirmTime { get; set; }
        public DateTime? UserUpdateTime { get; set; }
        public DateTime? UserConfirmTime { get; set; }
        public DateTime? DisinfectorUpdateTime { get; set; }
        public DateTime? DisinfectorConfirmTime { get; set; }
        public DateTime? DisinfectantUpdateTime { get; set; }
        public DateTime? DisinfectantConfirmTime { get; set; }
        public DateTime? PestUpdateTime { get; set; }
        public DateTime? PestConfirmTime { get; set; }
        public DateTime? DumpUpdateTime { get; set; }
        public DateTime? DumpConfirmTime { get; set; }
        public DateTime? ToiletUpdateTime { get; set; }
        public DateTime? ToiletConfirmTime { get; set; }
        public DateTime? VolunteerUpdateTime { get; set; }
        public DateTime? VolunteerConfirmTime { get; set; }
        public DateTime? LandfillUpdateTime { get; set; }
        public DateTime? LandfillConfirmTime { get; set; }
        public DateTime? IncineratorUpdateTime { get; set; }
        public DateTime? IncineratorConfirmTime { get; set; }
        public DateTime? DistrictUpdateTime { get; set; }
        public DateTime? DistrictConfirmTime { get; set; }
        
    }
}