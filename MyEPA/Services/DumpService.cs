using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MyEPA.Services
{
    public class DumpService
    {
        DumpRepository DumpRepository = new DumpRepository();
        public List<DumpModel> GetByFilter(DumpFilterParameter filter)
        {
            return DumpRepository.GetByFilter(filter);
        }

        public AdminResultModel Confirm(UserBriefModel user,int? townId)
        {
            DumpFilterParameter filter = new DumpFilterParameter 
            {
                CityIds = user.CityId.ToListCollection(),
            };

            switch(user.Duty)
            {
                case Enums.DutyEnum.Cleaning:
                    {
                        filter.TownIds = user.TownId.ToListCollection();
                    }
                    break;
                case Enums.DutyEnum.EPB:
                    {
                        if(townId.HasValue)
                        {
                            filter.TownIds = townId.Value.ToListCollection();
                        }
                    }
                    break;
            }

            List<DumpModel> dumps = DumpRepository.GetByFilter(filter
                );
            foreach (var item in dumps)
            {
                item.ConfirmTime = DateTimeHelper.GetCurrentTime();
            }
            DumpRepository.Update(dumps);
            return new AdminResultModel 
            {
                IsSuccess = true
            };
        }
        public void Update(DumpModel model)
        {
            DumpRepository.Update(model);
        }
        public DumpModel Get(int id)
        {
            return DumpRepository.Get(id);
        }

        public void Delete(string id)
        {
            DumpRepository.Delete(id);
        }

        public void Create(DumpModel model)
        {
            DumpRepository.Create(model);
        }
    }
}