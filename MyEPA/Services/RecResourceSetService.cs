using MyEPA.Extensions;
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
    public class RecResourceSetService : BaseEMISRepository<RecResourceSetModel>
    {
        RecResourceSetRepository RecResourceSetRepository = new RecResourceSetRepository();

        RecResourceRepository RecResourceRepository = new RecResourceRepository();

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

                    RecResourceIds = RecResourceId.ToListCollection(),
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
    }
}