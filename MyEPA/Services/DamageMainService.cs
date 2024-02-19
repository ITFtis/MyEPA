using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class DamageMainService
    {
        DamageMainRepository DamageMainRepository = new DamageMainRepository();
        public void UpdateByDone(int diasterId,int cityId)
        {
            DamageMainFilterParameter filter = new DamageMainFilterParameter 
            {
                CityIds = cityId.ToListCollection(),
                DiasterIds = diasterId.ToListCollection()
            };
            DamageMainModel main = DamageMainRepository.GetByFilter(filter);

            DateTime now = DateTimeHelper.GetCurrentTime();

            //沒有的話新增
            if (main == null)
            {
                DamageMainRepository.Create(new DamageMainModel 
                {
                    CityId = cityId,
                    DiasterId = diasterId,
                    DoneDate = now,
                    IsDone = true,
                    UpdateDate = now
                });
                return;
            }
            //有的話更新，
            //如果是已結案 => 取消
            //如果是未結案 => 結案
            if (main.IsDone)
            {
                main.IsDone = false;
                main.UpdateDate = now;
            }
            else
            {
                main.IsDone = true;
                main.UpdateDate = now;
                main.DoneDate = now;
            }
            DamageMainRepository.Update(main);
        }
    }
}