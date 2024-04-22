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

namespace MyEPA.Services
{
    public class RecResourceService
    {
        RecResourceRepository RecResourceRepository = new RecResourceRepository();        
        DiasterRepository DiasterRepository = new DiasterRepository();        
        UsersRepository UsersRepository = new UsersRepository();
        public List<RecResourceModel> GetByDiasterId(int diasterId, UserBriefModel user)
        {
            DiasterModel diaster = DiasterRepository.Get(diasterId);

            if (diaster == null)
            {
                return new List<RecResourceModel>();
            }

            ////List<int> cityIds = new List<int>() { user.CityId };
            RecResourceFilterParameter filter =
                new RecResourceFilterParameter
                {

                    ////CityIds =cityIds,
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
            model.CreateUser = user.UserName;
            model.CreateDate = DateTime.Now;            

            RecResourceRepository.Create(model);
        }
    }
}