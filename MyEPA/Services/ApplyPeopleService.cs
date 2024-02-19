using MyEPA.Enums;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Services
{
    /// <summary>
    /// 請求支援-人力
    /// </summary>
    public class ApplyPeopleService : ApplyBaseService<ApplyPeopleModel>, IApplyService<ApplyPeopleModel, ApplyPeopleViewModel>
    {
        private readonly ApplyPeopleRepository ApplyPeopleRepository = new ApplyPeopleRepository();
        private readonly ApplyPeopleHandlingSituationRepository ApplyPeopleHandlingSituationRepository = new ApplyPeopleHandlingSituationRepository();
        
        public ApplyPeopleModel GetCreateModel()
        {
            var returnModel = new ApplyPeopleModel()
            {
                RequireDate = DateTimeHelper.GetCurrentTime()
            };

            return returnModel;
        }

        public ApplyPeopleModel GetById(int id)
        {
            return ApplyPeopleRepository.Get(id);
        }
        public List<ApplyPeopleHandlingSituationModel> GetHandlingSituations(int id)
        {
            return ApplyPeopleHandlingSituationRepository.GetListByForeignkey(id);
        }
        public ApplyPeopleViewModel GetViewModelById(int id)
        {
            var found = ApplyPeopleRepository.GetByFilter(new ApplyPeopleFilterParameter()
            {
                Id = id
            }).FirstOrDefault();

            if (found == null)
            {
                return null;
            }

            return MappedViewModel(found);
        }

        public bool UpdateApplyPeopleEpaStatus(ApplyPeopleUpdateStatusViewModel request)
        {
            UpdateHandlingSituations(request);

            return base.UpdateEpaStatus(request);
        }

        private void UpdateHandlingSituations(ApplyPeopleUpdateStatusViewModel request)
        {
            List<ApplyPeopleHandlingSituationModel> handlingSituations =
                request.HandlingSituations.Select(e =>
                    new ApplyPeopleHandlingSituationModel
                    {
                        ApplyId = request.ApplyId,
                        Day = e.Day,
                        Quantity = e.Quantity,
                        Type = e.Type,
                        PeopleType = e.PeopleType,
                        Subsidy = e.Subsidy
                    }).ToList();
            ApplyPeopleHandlingSituationRepository.DeleteByForeignkey(request.ApplyId);
            ApplyPeopleHandlingSituationRepository.Create(handlingSituations);
        }

        public bool UpdateApplyPeopleEpbStatus(ApplyPeopleUpdateStatusViewModel request)
        {
            UpdateHandlingSituations(request);

            return base.UpdateEpbStatus(request);
        }

        private ApplyPeopleViewModel MappedViewModel(ApplyPeopleModel model, bool getFiledata = true)
        {
            var viewModel = new ApplyPeopleViewModel();
            var propertiess = typeof(ApplyPeopleModel).GetProperties();
            foreach (var property in propertiess)
            {
                var value = property.GetValue(model);
                property.SetValue(viewModel, value);
            }

            if (getFiledata)
            {
                var fileData = FileService.GetBySource(SourceTypeEnum.ApplyPeople, model.Id)
                                          .FirstOrDefault();
                if (fileData != null)
                {
                    viewModel.FileData = fileData;
                }
            }

            return viewModel;
        }

        public ApplyIndexViewModel<ApplyPeopleModel> GetApplyIndexViewModel(DutyEnum duty, ApplyRequestViewModel requestViewModel)
        {
            var result = new ApplyIndexViewModel<ApplyPeopleModel>();
            var filter = GetBaseFilter(duty, requestViewModel);
            var allAppliedRequests = ApplyPeopleRepository.GetByFilter(filter);

            if (allAppliedRequests.Count > 0)
            {
                result.ApplyStatus = $"本次災害已請求：清潔隊 {allAppliedRequests.Sum(c => c.CleaningMemberDays * c.CleaningMemberQuantity)} 人天、國軍 {allAppliedRequests.Sum(c => c.NationalArmyDays * c.NationalArmyQuantity)} 人天";
                result.AddAppliedRequests(allAppliedRequests);
            }

            if (string.IsNullOrEmpty(result.ApplyStatus))
            {
                result.ApplyStatus = "本次災害尚未有請求紀錄";
            }

            return result;
        }
        public ApplyViewModel GetApplyViewModel(DutyEnum duty, ApplyRequestViewModel requestViewModel)
        {
            var filter = GetBaseFilter(duty, requestViewModel);
            var model = 
                ApplyPeopleRepository.GetByFilter(filter).OrderByDescending(e => e.UpdateDate).FirstOrDefault();

            if (model == null)
            {
                return null;
            }

            var quantity = model.CleaningMemberQuantity * model.CleaningMemberDays
                + model.NationalArmyQuantity * model.NationalArmyDays;

            return new ApplyViewModel
            {
                Quantity = $"{quantity} (人天)",
                Status = model.IsToEpa ? model.EPBConfirmStatus.GetValueOrDefault().ToInteger() : model.EPAConfirmStatus.GetValueOrDefault().ToInteger()
            };
        }
        public bool Create(UserBriefModel user, ApplyPeopleModel model, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
        {
            SetBasicCreateModel(ref model, user, requestViewModel);
            var id = ApplyPeopleRepository.CreateAndResultIdentity<int>(model);

            FileService.UploadFileByGuidName(new UploadFileBaseModel
            {
                File = file,
                SourceId = id,
                SourceType = SourceTypeEnum.ApplyPeople,
                User = user.UserName
            }, isShowError: false);

            return true;
        }

        public bool Edit(UserBriefModel user, ApplyPeopleModel model, HttpPostedFileBase file)
        {
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateUser = user.UserName;

            ApplyPeopleRepository.Edit(model);

            base.UpdateStatus(user, model);

            if (file != null)
            {
                FileService.DeleteFileBySource(SourceTypeEnum.ApplyPeople, model.Id);
                FileService.UploadFileByGuidName(new UploadFileBaseModel
                {
                    File = file,
                    SourceId = model.Id,
                    SourceType = SourceTypeEnum.ApplyPeople,
                    User = user.UserName
                });
            }

            return true;
        }


        public void Delete(UserBriefModel user, int id)
        {
            var found = GetById(id);
            if (found == null)
            {
                throw new Exception("查無對應請求單");
            }

            //若為清潔隊則檢查是否是自己的單子
            if (user.Duty == DutyEnum.Cleaning)
            {
                if (found.CreateUser != found.CreateUser)
                {
                    throw new Exception("無法刪除不是自己建立的請求單");
                }
            }

            ApplyPeopleRepository.Delete(id);
        }

        public List<ApplyPeopleViewModel> GetApplyViewModelsByFilter(ApplyBaseFilterParameter filter)
        {
            var applyRequests = ApplyPeopleRepository.GetByFilter(filter);
            var viewModels = applyRequests.Select(c => MappedViewModel(c, false))
                                          .ToList();
            return viewModels;
        }

        /// <summary>
        /// 取得審核用 model, 若狀態為未處理會改成已處理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApplySupportProcessingDetailViewModel GetApplySupportProcessingDetailViewModel(int id)
        {
            var viewModel = GetViewModelById(id);
            if (viewModel != null)
            {
                var returnViewModel = new ApplySupportProcessingDetailViewModel(viewModel);
                var des = $"清潔隊： {viewModel.CleaningMemberQuantity} 人 * {viewModel.CleaningMemberDays} 天，國軍： {viewModel.NationalArmyQuantity} 人 * {viewModel.NationalArmyDays} 天";
                returnViewModel.ReqeustDescirptions.Add(des);

                //若 EPBConfirmStatus 是 null 或 pending 要轉為審核中
                if (returnViewModel.EPBConfirmStatus == null || returnViewModel.EPBConfirmStatus == ApplyStatusEnum.Pending)
                {
                    ApplyPeopleRepository.UpdateEpbConfrimStatusToProcess(returnViewModel.Id);
                    returnViewModel.EPBConfirmStatus = ApplyStatusEnum.Processing;
                    returnViewModel.EPBConfirmUpdateTime = DateTime.Now;
                }

                //若 EPBConfirmStatus 是 null 或 pending 要轉為審核中
                if (returnViewModel.IsToEpa && (returnViewModel.EPAConfirmStatus.HasValue == false || returnViewModel.EPAConfirmStatus.Value == ApplyStatusEnum.Pending))
                {
                    ApplyPeopleRepository.UpdateEpaConfrimStatusToProcess(returnViewModel.Id);
                    returnViewModel.EPAConfirmStatus = ApplyStatusEnum.Processing;
                    returnViewModel.EPAConfirmUpdateTime = DateTimeHelper.GetCurrentTime();
                }
                return returnViewModel;
            }

            return null;
        }
    }
}