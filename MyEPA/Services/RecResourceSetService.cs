using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Services
{
    public class RecResourceSetService
    {
        RecResourceSetRepository RecResourceSetRepository = new RecResourceSetRepository();

        RecResourceRepository RecResourceRepository = new RecResourceRepository();

        public RecResourceSetModel Get(int id)
        {
            var model = RecResourceSetRepository.Get(id);

            return model;
        }

        public List<RecResourceSetModel> GetByRecResourceIdNeed(int RecResourceIdNeed)
        {
            RecResourceSetFilterParameter filter =
                new RecResourceSetFilterParameter
                {

                    RecResourceIdNeeds = RecResourceIdNeed.ToListCollection(),
                };

            var recResource = RecResourceSetRepository
                .GetByFilter(filter);

            return recResource;
        }

        public List<RecResourceSetModel> GetByRecResourceIdHelp(int RecResourceIdHelp)
        {
            RecResourceSetFilterParameter filter =
                new RecResourceSetFilterParameter
                {

                    RecResourceIdHelps = RecResourceIdHelp.ToListCollection(),                    
                };

            var recResource = RecResourceSetRepository
                .GetByFilter(filter);

            return recResource;
        }

        public List<RecResourceSetModel> GetByRecResourceId(int RecResourceId)
        {
            RecResourceModel rec = RecResourceRepository.Get(RecResourceId);

            if (rec == null)
            {
                return new List<RecResourceSetModel>();
            }

            RecResourceSetFilterParameter filter =
                new RecResourceSetFilterParameter
                {

                    RecResourceIdNeeds = RecResourceId.ToListCollection(),
                    //Types = user.Duty == DutyEnum.Water ? WaterCheckTypeEnum.Water.ToListCollection() : WaterCheckTypeEnum.EPPersonnel.ToListCollection()
                };

            var recResource = RecResourceSetRepository
                .GetByFilter(filter);

            return recResource;
        }

        public void Create(UserBriefModel user, RecResourceSetModel model)
        {
            model.CreateUser = user.UserName;
            model.CreateDate = DateTime.Now;

            RecResourceSetRepository.Create(model);
        }

        public void Update(UserBriefModel user, RecResourceSetModel model)
        {
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateUser = user.UserName;

            RecResourceSetRepository.Update(model);
        }

        public AdminResultModel Delete(int id)
        {
            var entity = RecResourceSetRepository.Get(id);

            if (entity == null)
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "資料不存在"
                };

            RecResourceSetRepository.Delete(id);

            return new AdminResultModel
            {
                IsSuccess = true
            };
        }
    }
}