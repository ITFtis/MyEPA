using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.BaseModels;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.Repositories.BaseRepositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class ApplyBaseService<T> where T : ApplyBaseModel, new()
    {
        internal readonly FileDataService FileService = new FileDataService();
        internal readonly ApplyBaseRepositroy<T> applyBaseRepositroy = new ApplyBaseRepositroy<T>();
        internal readonly ApplyHandlingSituationRepositroy ApplyHandlingSituationRepositroy = new ApplyHandlingSituationRepositroy();
        private ApplyTypeEnum GetApplyType()
        {
            var typeName = typeof(T).Name;
            foreach (var item in ExtensionsOfEnum.GetEnumAllValue<ApplyTypeEnum>())
            {
                if (typeName.Contains(item.ToString()))
                {
                    return item;
                }
            }
            throw new NotImplementedException();
        }
        public List<ApplyHandlingSituationModel> GetHandlingSituations(int id)
        {
            return ApplyHandlingSituationRepositroy.GetByApply(GetApplyType(),id);
        }
        public static ApplyBaseFilterParameter GetBaseFilter(DutyEnum duty, ApplyRequestViewModel requestViewModel) 
        {

            var filter = new ApplyBaseFilterParameter()
            {
            };

            if (requestViewModel.DiasterId.HasValue) 
            {
                filter.DiasterIds = new List<int>() { requestViewModel.DiasterId.Value };
            }

            filter.CityIds = new List<int>() { requestViewModel.CityId.Value };
            filter.TownIds = new List<int>() { requestViewModel.TownId.Value };

            return filter;
        }

        public T GetBasicCreateModel(UserBriefModel user, ApplyRequestViewModel requestViewModel) 
        {
            var model = new T();
            SetBasicCreateModel(ref model, user, requestViewModel);

            return model;
        }

        public void SetBasicCreateModel(ref T model, UserBriefModel user, ApplyRequestViewModel requestViewModel)
        {
            //環保局新增直接轉呈環保署
            if(user.Duty == DutyEnum.EPB)
            {
                model.IsToEpa = true;
                model.EPAConfirmStatus = ApplyStatusEnum.Pending;
            }
            else
            {
                model.EPBConfirmStatus = ApplyStatusEnum.Pending;
            }
            model.CityId = requestViewModel.CityId.Value;
            model.TownId = requestViewModel.TownId.Value;
            model.DiasterId = requestViewModel.DiasterId.Value;
            model.CreateDate = DateTimeHelper.GetCurrentTime();
            model.CreateUser = user.UserName;
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateUser = user.UserName;
            model.Status = ApplyStatusEnum.Pending;
        }
        public bool UpdateStatus(UserBriefModel user, ApplyBaseModel apply)
        {
            if (apply == null)
            {
                throw new ArgumentNullException(nameof(apply));
            }

            var entity = applyBaseRepositroy.Get(apply.Id);

            if (entity == null)
            {
                return false;
            }

            if (entity.Status == ApplyStatusEnum.Reject
                || entity.EPAConfirmStatus == ApplyStatusEnum.Reject
                || entity.EPBConfirmStatus == ApplyStatusEnum.Reject
                )
            {
                //環保局直接轉呈環保署
                if (user.Duty == DutyEnum.EPB)
                {
                    entity.IsToEpa = true;
                    entity.EPAConfirmStatus = ApplyStatusEnum.Pending;
                }
                else
                {
                    entity.EPBConfirmStatus = ApplyStatusEnum.Pending;
                }
                entity.Status = ApplyStatusEnum.Pending;

                try
                {
                    applyBaseRepositroy.Update(entity);
                }
                catch
                {
                    throw;
                }
            }

            return true;
        }
        public bool UpdateEpbStatus(ApplySupportUpdateStatusViewModel request, List<ApplyHandlingSituationViewModel> handlingSituations)
        {
            UpdateHandlingSituations(request.ApplyId, handlingSituations);
            return UpdateEpbStatus(request);
        }
        public bool UpdateEpaStatus(ApplySupportUpdateStatusViewModel request, List<ApplyHandlingSituationViewModel> handlingSituations)
        {
            UpdateHandlingSituations(request.ApplyId, handlingSituations);
            return UpdateEpaStatus(request);
        }
        private void UpdateHandlingSituations(int applyId,List<ApplyHandlingSituationViewModel> handlingSituations)
        {
            var applyType = GetApplyType();

            ApplyHandlingSituationRepositroy.DeleteByApply(applyType, applyId);
            
            if (handlingSituations.IsNotEmpty())
            {
                List<ApplyHandlingSituationModel> list =
                handlingSituations.Select(e =>
                   new ApplyHandlingSituationModel
                   {
                       ApplyId = applyId,
                       Type = e.Type,
                       Subsidy = e.Subsidy,
                       ApplyType = applyType.ToInteger(),
                       Description = e.Description
                   }).ToList();
                ApplyHandlingSituationRepositroy.Create(list);
            }
        }
        public bool UpdateEpaStatus(ApplySupportUpdateStatusViewModel request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var apply = applyBaseRepositroy.Get(request.ApplyId);

            if (apply == null)
            {
                return false;
            }

            apply.EPAConfirmStatus = ApplyStatusEnum.Pending;
            apply.EPAConfirmUpdateTime = DateTimeHelper.GetCurrentTime();

            if (request.CheckConfirm)
            {
                apply.EPAConfirmStatus = ApplyStatusEnum.Confrim;
                apply.EPAConfirmDescribe = request.TextConfirm;
                apply.Status = ApplyStatusEnum.Confrim;
            }

            if (request.CheckReject)
            {
                apply.EPAConfirmStatus = ApplyStatusEnum.Reject;
                apply.EPAConfirmDescribe = request.TextReject;
                apply.Status = ApplyStatusEnum.Reject;
            }

            try
            {
                applyBaseRepositroy.Update(apply);
            }
            catch
            {
                throw;
            }

            return true;
        }
        public bool UpdateEpbStatus(ApplySupportUpdateStatusViewModel request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var apply = applyBaseRepositroy.Get(request.ApplyId);

            if(apply == null)
            {
                return false;
            }

            apply.EPBConfirmStatus = ApplyStatusEnum.Pending;
            apply.EPBConfirmUpdateTime = DateTimeHelper.GetCurrentTime();

            if (request.CheckConfirm) 
            {
                apply.EPBConfirmStatus = ApplyStatusEnum.Confrim;
                apply.EPBConfirmDescribe = request.TextConfirm;
                apply.Status = ApplyStatusEnum.Confrim;
            }

            if (request.CheckReject)
            {
                apply.EPBConfirmStatus = ApplyStatusEnum.Reject;
                apply.EPBConfirmDescribe = request.TextReject;
                apply.Status = ApplyStatusEnum.Reject;
            }

            if (request.CheckSendToEPA)
            {
                apply.EPBConfirmStatus = ApplyStatusEnum.SendToEpa;
                apply.EPBConfirmDescribe = request.TextSendToEPA;
                apply.IsToEpa = true;
                apply.EPAConfirmStatus = ApplyStatusEnum.Pending;
                apply.Status = ApplyStatusEnum.SendToEpa;
            }

            try
            {
                applyBaseRepositroy.Update(apply);
            }
            catch 
            {
                throw;
            }

            return true;
        }
        public T Get(ApplyRequestViewModel request)
        {
            return applyBaseRepositroy.GetByFilter(new ApplyBaseFilterParameter
            {
                CityIds = request.CityId.HasValue ? request.CityId.Value.ToListCollection() : new List<int>(),
                DiasterIds = request.DiasterId.HasValue ? request.DiasterId.Value.ToListCollection() : new List<int>(),
                TownIds = request.TownId.HasValue ? request.TownId.Value.ToListCollection() : new List<int>(),
            }).FirstOrDefault();
        }
    }
}