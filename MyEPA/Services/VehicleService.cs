using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MyEPA.Services
{
    public class VehicleService
    {
        VehicleRepository VehicleRepository = new VehicleRepository();
        VehicleTypeRepository VehicleTypeRepository = new VehicleTypeRepository();
        public List<VehicleModel> GetByFilter(VehicleFilterParameter filter)
        {
            return VehicleRepository.GetByFilter(filter);
        }
        public List<VehicleReportModel> GetReportByFilter(VehicleFilterParameter filter)
        {
            return VehicleRepository.GetReportByFilter(filter);
        }

        public List<VehicleCountModel> GetCarsCountByCity()
        {
            return VehicleRepository.GetCarsCountByCity();
        }

        public List<string> GetCarTypes()
        {
            List<string> VehicleType=new List<string>();
            VehicleType.Add("垃圾車");
            VehicleType.AddRange(VehicleTypeRepository.GetList().GroupBy(e => e.Name).Select(e => e.Key).ToList());
            VehicleType.RemoveAll(s => s == "子母式垃圾車" || s == "密封式壓縮垃圾車" || s == "密封式轉運垃圾車" || s == "密封式垃圾車" || s == "其他框式垃圾車");
            return VehicleType;
        }
    }
}