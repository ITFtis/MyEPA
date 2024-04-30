using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models.FilterParameter;
using MyEPA.Models;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyEPA.Helper;
using MyEPA.ViewModels;
using System.Web.ApplicationServices;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using System.Web.UI.WebControls;

namespace MyEPA.Services
{
    public class RecResourceService
    {
        RecResourceRepository RecResourceRepository = new RecResourceRepository();        
        DiasterRepository DiasterRepository = new DiasterRepository();        
        UsersRepository UsersRepository = new UsersRepository();

        public string GetAlter()
        {
            string result = "";

            try
            {
                var diaster = new DiasterService().GetAll().FirstOrDefault();
                var recs = GetByDiasterId(diaster.Id);

                recs = recs.Where(a => a.Type == 1)
                        .Where(a => a.Status == 1).ToList();
                if (recs.Count > 0)
                {
                    string str = string.Format("{0} (資源調度未結案：{1}筆)", diaster.DiasterName, recs.Count);
                    result = str;
                }
            }
            catch (Exception ex)
            {
                return "";
            }            

            return result;
        }

        public List<RecResourceModel> GetByDiasterId(int diasterId)
        {
            DiasterModel diaster = DiasterRepository.Get(diasterId);

            if (diaster == null)
            {
                return new List<RecResourceModel>();
            }
            
            RecResourceFilterParameter filter =
                new RecResourceFilterParameter
                {
                    
                    DiasterIds = diasterId.ToListCollection(),
                    //Types = user.Duty == DutyEnum.Water ? WaterCheckTypeEnum.Water.ToListCollection() : WaterCheckTypeEnum.EPPersonnel.ToListCollection()
                };

            var recResource = RecResourceRepository
                .GetByFilter(filter);

            return recResource;

            ////List<string> userNames = new List<string>();
            ////if (user.Duty == DutyEnum.Water)
            ////{
            ////    userNames = UsersRepository.GetListByFilter(new UsersBriefFilterParameter
            ////    {
            ////        ////CityIds = user.CityId.ToListCollection(),
            ////        TownIds = user.TownId.ToListCollection()
            ////    }).Select(e => e.UserName).ToList();
            ////}
        }

        public void Create(UserBriefModel user, RecResourceModel model)
        {
            model.Status = 1;
            model.CreateUser = user.UserName;
            model.CreateDate = DateTime.Now;            

            RecResourceRepository.Create(model);
        }

        public RecResourceModel Get(int id)
        {
            var model = RecResourceRepository.Get(id);

            return model;
        }

        public List<RecResourceModel> GetByCityId(int cityId)
        {            
            return RecResourceRepository.GetByCityId(cityId);
        }

        public void Update(UserBriefModel user, RecResourceModel model)
        {
            var entity = RecResourceRepository.Get(model.Id);

            if (entity == null)
                return;

            entity.CityId = model.CityId;
            entity.Reason = model.Reason;
            entity.ContactPerson = model.ContactPerson;
            entity.ContactMobilePhone = model.ContactMobilePhone;
            entity.Items = model.Items;
            entity.Spec = model.Spec;
            entity.Quantity = model.Quantity;
            entity.Unit = model.Unit;
            entity.USDate = model.USDate;
            entity.UEDate = model.UEDate;

            entity.UpdateDate = DateTimeHelper.GetCurrentTime();
            entity.UpdateUser = user.UserName;

            RecResourceRepository.Update(entity);
        }

        public AdminResultModel Delete(int id)
        {
            var entity = RecResourceRepository.Get(id);

            if (entity == null)
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "資料不存在"
                };

            RecResourceRepository.Delete(id);

            return new AdminResultModel
            {
                IsSuccess = true
            };
        }

        public AdminResultModel Finish(int id)
        {
            var entity = RecResourceRepository.Get(id);

            if (entity == null)
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "資料不存在"
                };

            entity.Status = 2;
            RecResourceRepository.Update(entity);

            return new AdminResultModel
            {
                IsSuccess = true
            };
        }
    }
}