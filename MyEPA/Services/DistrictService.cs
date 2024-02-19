using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.JsnoModel;
using MyEPA.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{

    public class DistrictService
    {
        DistrictRepository DistrictRepository = new DistrictRepository();

        public void Update(UserBriefModel user, DistrictModel model)
        {
            var districtName = $"{user.City}{user.Town}";

            DistrictModel entity = DistrictRepository.GetByDistrictName(districtName);

            if(entity == null)
            {
                return;
            }

            entity.DistrictName = districtName;
            entity.Address = model.Address;
            entity.CleanCapacity = model.CleanCapacity;
            entity.CoverArea = model.CoverArea;
            //entity.DistrictName = model.DistrictName;
            entity.Fax = model.Fax;
            entity.Human = model.Human;
            entity.Mail = model.Mail;
            entity.OutHuman = model.OutHuman;
            entity.Phone = model.Phone;
            entity.ReadyHuman = model.ReadyHuman;
            entity.Service = model.Service;
            entity.UpdateTime = DateTimeHelper.GetCurrentTime();
            entity.ConfirmTime = DateTimeHelper.GetCurrentTime();
            DistrictRepository.Update(entity);
        }

        public DistrictModel GetByDistrictName(string districtName)
        {
            return DistrictRepository.GetByDistrictName(districtName);
        }
    }
}