using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyEPA.Services
{
    public class DefendService
    {
        DefendRepository DefendRepository = new DefendRepository();
        DefendQuestionRepository DefendQuestionRepository = new DefendQuestionRepository();
        DiasterRepository DiasterRepository = new DiasterRepository();
        TownRepository TownRepository = new TownRepository();
        CityRepository CityRepository = new CityRepository();
        public List<DefendTeamConfirmViewModel> GetConfirmList(int diasterId, AreaEnum area)
        {
            var citys = CityRepository.GetListByFilter(new CityFilterParameter
            {
                AreaIds = area.ToInteger().ToListCollection(),
                IsCounty = true
            }).ToList();

            var towns = TownRepository.GetListByFilter(new TownFilterParameter
            {
                CityIds = citys.Select(e => e.Id).ToList(),
                IsTown = false
            }).ToList();

            List<DefendModel> defends =
               DefendRepository.GetByFilter(new DefendParameter
               {
                   DutyIds = DutyEnum.EPB.ToInteger().ToListCollection(),
                   TownIds = towns.Select(e => e.Id).ToList(),
                   DiasterIds = diasterId.ToListCollection()
               });

            List<DefendTeamConfirmViewModel> result = new List<DefendTeamConfirmViewModel>();

            foreach (TownModel town in towns)
            {
                DefendModel defend = defends.FirstOrDefault(e => e.TownId == town.Id);
                result.Add(new DefendTeamConfirmViewModel
                {
                    CityId = town.CityId,
                    CityName = citys.Where(e => e.Id == town.CityId).Select(e => e.City).FirstOrDefault(),
                    Status = defend == null ? DefendStatusEnum.UnNotification : (DefendStatusEnum)defend.Status,
                    Id = defend?.Id
                });
            }

            return result;
        }
        public List<DefendConfirmViewModel> GetConfirmList(int diasterId, int cityId)
        {
            List<DefendModel> defends =
                DefendRepository.GetByFilter(new DefendParameter
                {
                    DutyIds = DutyEnum.Cleaning.ToInteger().ToListCollection(),
                    CityIds = cityId.ToListCollection(),
                    DiasterIds = diasterId.ToListCollection()
                });

            var towns = TownRepository.GetByCityId(cityId)
                .ToDictionary(e => e.Id, e => e.Name);

            Dictionary<int, List<DefendDutyQuestionJoinModel>> questions =
                DefendQuestionRepository.GetByDiasterId(diasterId, cityId)
                .GroupBy(e => e.DefendId.Value)
                .ToDictionary(e => e.Key, e => e.ToList());

            return defends.Select(e => new DefendConfirmViewModel 
            {
                Id = e.Id,
                Questions = questions.GetValue(e.Id,new List<DefendDutyQuestionJoinModel> { }),
                Status = (DefendStatusEnum)e.Status,
                UpdateReason = e.UpdateReason,
                TownName = towns.GetValue(e.TownId,string.Empty)
            }).ToList();
        }
        public List<DefendReportModel> GetReport(DefendReportFilterModel filter)
        {
            return DefendRepository.GetReport(filter);
        }
        public List<DefendTownReportModel> GetTownReport(DefendTownReportFilterModel filter)
        {
            return DefendRepository.GetTownReport(filter);
        }
        public DefendViewModel Get(string userName, DefendViewModel model)
        {
            TownModel town = TownRepository.Get(model.TownId);

            DutyEnum duty = DutyEnum.Cleaning;

            if (town?.Name == "環保局")
            {
                duty = DutyEnum.EPB;
            }

            int dutyId = duty.ToInteger();

            DefendModel defend = DefendRepository.Get(dutyId, model.DiasterId.GetValueOrDefault(), model.CityId, model.TownId);

            if (defend == null)
            {
                model.Questions = DefendQuestionRepository.GetByDefendId(dutyId);
            }
            else
            {
                model.Status = (DefendStatusEnum)defend.Status;
                model.Questions = DefendQuestionRepository.GetByDefendId(dutyId, defend.Id);
                model.Id = defend.Id;
            }

            return model;
        }

        public List<DefendTownQuestionModel> GetTownAns(int diasterId, int? townId,int? cityId)
        {
            return DefendRepository.GetDefendTownQuestions(diasterId, townId, cityId).OrderBy(e => e.TownName).ThenBy(e => e.Sort).ToList();
        }

        /// <summary>
        /// 取得尚未通報
        /// </summary>
        public List<UnNotificationJoinDefendModel> GetUnNotifications(int diasterId, int cityId)
        {
            return DefendRepository.GetUnNotifications(diasterId, cityId);
        }

        public void CreateOrUpdate(string userName, DefendViewModel model)
        {
            DiasterModel diaster = DiasterRepository.GetByRuning();

            if(diaster == null)
            {
                throw new Exception("無開啟的災害");
            }

            TownModel town = TownRepository.Get(model.TownId);

            DutyEnum duty = DutyEnum.Cleaning;

            if (town?.Name == "環保局")
            {
                duty = DutyEnum.EPB;
            }

            var defend = DefendRepository.Get(duty.ToInteger(), diaster.Id, model.CityId, model.TownId);

            
            Func<DefendDutyQuestionJoinModel, int, DefendQuestionModel> convertToModel = 
                (intput, defendId) =>
                new DefendQuestionModel
                {
                    Id = intput.Id.GetValueOrDefault(),
                    Ans = intput.Ans.GetValueOrDefault(),
                    DefendId = defendId,
                    QuestionId = intput.QuestionId,
                    Remark = intput.Remark,
                    UpdateDate = DateTimeHelper.GetCurrentTime()
                };

            if (defend == null)
            {
                DefendModel entity = new DefendModel
                {
                    CityId = model.CityId,
                    TownId = model.TownId,
                    Status = DefendStatusEnum.Waiting.ToInteger(),
                    CreateDate = DateTimeHelper.GetCurrentTime(),
                    Creator = userName,
                    UpdateDate = DateTimeHelper.GetCurrentTime(),
                    Confirmor = null,
                    ConfirmTime = null,
                    DiasterId = diaster.Id,
                    DutyId = duty.ToInteger(),
                    Updator = userName,
                    UpdateReason = string.Empty,
                };

                int defendId =  DefendRepository.CreateAndResultIdentity<int>(entity);
                
                var questions =
                    model.Questions.Select(e => convertToModel(e, defendId)).ToList();
                DefendQuestionRepository.Create(questions);
                return;
            }
            //確認後不能修改
            if (defend.Status == DefendStatusEnum.Confirm.ToInteger())
            {
                return;
            }
            else
            {
                DefendQuestionRepository.DeleteByDefendId(defend.Id);
                
                var questions = model.Questions.Select(e => convertToModel(e, defend.Id)).ToList();
                defend.UpdateDate = DateTimeHelper.GetCurrentTime();
                defend.Updator = userName;
                defend.UpdateReason = model.UpdateReason;
                DefendQuestionRepository.CreateOrUpdate(questions);
                DefendRepository.Update(defend);
            }
            
        }

        public void Confirm(UserBriefModel user, int id, DefendStatusEnum status)
        {
            var result = DefendRepository.Get(id);

            //無資料或是確認過不回應
            if (result == null)
            {
                return;
            }

            result.Status = status.ToInteger();

            result.ConfirmTime = DateTimeHelper.GetCurrentTime();

            result.Confirmor = user.UserName;

            DefendRepository.Update(result);
        }
    }
}