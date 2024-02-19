using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class ToiletLocationService
    {
        ToiletLocationRepository ToiletLocationRepository = new ToiletLocationRepository();
        CityRepository CityRepository = new CityRepository();
        TownRepository TownRepository = new TownRepository();
        
        public List<ToiletLocationManagementTownModel> GetManagementTownByFilter(ToiletLocationFilterParameter filter)
        {
            return ToiletLocationRepository.GetManagementTownByFilter(filter);
        }

        public List<int> GetDiasterIds()
        {
            return ToiletLocationRepository.GetDiasterIds(new ToiletLocationFilterParameter { });
        }

        public List<ToiletLocationViewModel> GetByFilter(ToiletLocationFilterParameter filter)
        {
            var citys = CityRepository.GetList().ToDictionary(e => e.Id, e => e.City);
            var towns = TownRepository.GetList().ToDictionary(e => e.Id, e => e.Name);

            return ToiletLocationRepository.GetByFilter(filter)
                .Select(e => new ToiletLocationViewModel
                {
                    Address = e.Address,
                    CityId = e.CityId,
                    AddressDisplay = $"{citys.GetValue(e.CityId)}{towns.GetValue(e.TownId)}{e.Address}",
                    ContactMethod = e.ContactMethod,
                    ContactPerson = e.ContactPerson,
                    CreateDate = e.CreateDate,
                    CreateUser = e.CreateUser,
                    DateDisplay = $"{e.StartDate.ToDateString()}-{e.EndDate.ToDateString()}",
                    DiasterId = e.DiasterId,
                    EndDate = e.EndDate,
                    Id = e.Id,
                    ManagementTownDisplay = $"{citys.GetValue(e.CityId)}{towns.GetValue(e.ManagementTownId)}",
                    ManagementTownId = e.ManagementTownId,
                    Note = e.Note,
                    StartDate = e.StartDate,
                    ToiletQuantity = e.ToiletQuantity,
                    ToiletType = e.ToiletType,
                    TownId = e.TownId,
                    UpdateDate = e.UpdateDate,
                    UpdateUser = e.UpdateUser,
                    Xpos = e.Xpos,
                    Ypos = e.Ypos
                })
                .ToList();
        }

        public List<ToiletLocationStatisticsViewModel> GetStatisticsByFilter(ToiletLocationFilterParameter filter)
        {
            return ToiletLocationRepository.GetStatisticsByFilter(filter)
               .Select(e => new ToiletLocationStatisticsViewModel
               {
                   Address = e.Address,
                   CityName = e.CityName,
                   ContactMethod = e.ContactMethod,
                   ContactPerson = e.ContactPerson,
                   DateDisplay = $"{e.StartDate.ToDateString()}-{e.EndDate.ToDateString()}",
                   Id = e.Id,
                   Note = e.Note,
                   ToiletQuantity = e.ToiletQuantity,
                   LastCleanDate = e.LastCleanDate,
                   ToiletType = e.ToiletType,
                   TownName = e.TownName,
                   EndDate = e.EndDate,
                   StartDate = e.StartDate
               })
               .ToList();
        }

        public ToiletLocationModel Get(int id)
        {
            return ToiletLocationRepository.Get(id);
        }

        public AdminResultModel Delete(int id)
        {
            ToiletLocationRepository.Delete(id);
            return new AdminResultModel { IsSuccess = true };
        }

        public AdminResultModel CreateOrUpdate(ToiletLocationModel model)
        {
            ToiletLocationModel entity = ToiletLocationRepository.Get(model.Id);

            if (entity == null)
            {
                entity = new ToiletLocationModel
                {
                    CreateDate = DateTimeHelper.GetCurrentTime(),
                    CreateUser = model.UpdateUser,
                    DiasterId = model.DiasterId,
                };
            }

            entity.Address = model.Address;
            entity.ContactMethod = model.ContactMethod;
            entity.TownId = model.TownId;
            entity.CityId = model.CityId;
            entity.ContactPerson = model.ContactPerson;
            entity.EndDate = model.EndDate;
            entity.ManagementTownId = model.ManagementTownId;
            entity.Note = model.Note;
            entity.StartDate = model.StartDate;
            entity.ToiletQuantity = model.ToiletQuantity;
            entity.ToiletType = model.ToiletType;
            entity.UpdateDate = DateTimeHelper.GetCurrentTime();
            entity.UpdateUser = model.UpdateUser;
            entity.Xpos = model.Xpos;
            entity.Ypos = model.Ypos;

            ToiletLocationRepository.CreateOrUpdate(entity);
            return new AdminResultModel 
            {
                IsSuccess = true
            };
        }
    }
}