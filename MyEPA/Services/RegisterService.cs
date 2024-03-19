using Microsoft.Win32;
using MyEPA.Models;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.Windows.Interop;

namespace MyEPA.Services
{
    public class RegisterService
    {
        RegistersRepository registersRepository = new RegistersRepository();
        UsersRepository userRepository = new UsersRepository();

        public AdminResultModel Register(RegistersModel model, List<string> humanType)
        {
            if (userRepository.GetByUserName(model.Id) != null)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "抱歉，與現有用戶同帳號，請換帳號名稱後申請"
                };
            }

            if (registersRepository.Get(model.Id) != null)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "抱歉，此帳號已經存在，待審核中！"
                };
            }

            model.HumanType = string.Join("、", humanType ?? new List<string>());

            try
            {

                if (!string.IsNullOrWhiteSpace(model.Duty))
                {
                    var foundDuty = new DutyRepository().GetByDutyName(model.Duty);
                    model.DutyId = foundDuty?.Id;
                }

                if (!string.IsNullOrWhiteSpace(model.City))
                {
                    var foundCity = new CityRepository().GetByCityName(model.City);
                    model.CityId = foundCity?.Id;
                }

                if (model.CityId.HasValue && !string.IsNullOrWhiteSpace(model.Town))
                {
                    var foundTown = new TownRepository().GetByTownName(model.CityId.Value, model.Town);
                    model.TownId = foundTown?.Id;

                    if(model.TownId == null)
                    {
                        return new AdminResultModel
                        {
                            IsSuccess = false,
                            ErrorMessage = "抱歉，此鄉鎮並不存在資料表，請通知系統管理員！" + model.Town
                        };
                    }
                }

                if ((model.CityId != null && model.TownId != null))
                {
                    UsersRepository UsersRepository = new UsersRepository();
                    if (model.MainContacter == "是" && UsersRepository.IsExistsByMainContacter((int)model.CityId, (int)model.TownId))
                    {

                        return new AdminResultModel
                        {
                            IsSuccess = false,
                            ErrorMessage = "抱歉，主要負責人已存在，不可重覆設定：" + model.City + model.Town
                        };
                    }
                }

                registersRepository.Create(model);
            }
            catch (Exception ex)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "註冊失敗"
                };
            }

            return new AdminResultModel
            {
                IsSuccess = true,
                ErrorMessage = "已登錄成功，請等待審核"
            };
        }
    }
}