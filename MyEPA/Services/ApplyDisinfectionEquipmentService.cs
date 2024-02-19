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
    /// 請求支援-消毒設備
    /// </summary>
    public class ApplyDisinfectionEquipmentService : ApplyBaseService<ApplyDisinfectionEquipmentModel>, IApplyService<ApplyDisinfectionEquipmentModel, ApplyDisinfectionEquipmentViewModel>
    {
        private readonly ApplyDisinfectionEquipmentHandlingSituationRepository ApplyDisinfectionEquipmentHandlingSituationRepository = new ApplyDisinfectionEquipmentHandlingSituationRepository();
        public new List<ApplyDisinfectionEquipmentHandlingSituationModel> GetHandlingSituations(int id)
        {
            return ApplyDisinfectionEquipmentHandlingSituationRepository.GetListByForeignkey(id);
        }
        public ApplyViewModel GetApplyViewModel(DutyEnum duty, ApplyRequestViewModel requestViewModel)
        {
            var filter = GetBaseFilter(duty, requestViewModel);
            var model =
                ApplyDisinfectionEquipmentRepository.GetByFilter(filter).OrderByDescending(e => e.UpdateDate).FirstOrDefault();

            if (model == null)
            {
                return null;
            }

            MappedViewModel(model);

            return new ApplyViewModel
            {
                Quantity = $"{model.Details.Sum(e => e.Quantity)} 單位",
                Status = model.IsToEpa ? model.EPBConfirmStatus.GetValueOrDefault().ToInteger() : model.EPAConfirmStatus.GetValueOrDefault().ToInteger()
            };
        }
        private readonly ApplyDisinfectionEquipmentRepository ApplyDisinfectionEquipmentRepository = new ApplyDisinfectionEquipmentRepository();
        public bool Create(UserBriefModel user, ApplyDisinfectionEquipmentModel model, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
        {
            SetBasicCreateModel(ref model, user, requestViewModel);

            var id = ApplyDisinfectionEquipmentRepository.Create(model);

            if (file != null)
            {
                FileService.UploadFileByGuidName(new UploadFileBaseModel
                {
                    File = file,
                    SourceId = id,
                    SourceType = SourceTypeEnum.ApplyDisinfectionEquipment,
                    User = user.UserName
                });
            }

            return true;
        }

        public void Delete(UserBriefModel user, int id)
        {
            var found = ApplyDisinfectionEquipmentRepository.Get(id);
            if (found == null)
            {
                throw new Exception("Data not found");
            }
            ApplyDisinfectionEquipmentRepository.DeleteDetails(id);
            ApplyDisinfectionEquipmentRepository.Delete(id);
        }

        public bool Edit(UserBriefModel user, ApplyDisinfectionEquipmentModel model, HttpPostedFileBase file)
        {
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateUser = user.UserName;

            ApplyDisinfectionEquipmentRepository.Edit(model);

            base.UpdateStatus(user, model);

            if (file != null)
            {
                FileService.DeleteFileBySource(SourceTypeEnum.ApplyDisinfectionEquipment, model.Id);
                FileService.UploadFileByGuidName(new UploadFileBaseModel
                {
                    File = file,
                    SourceId = model.Id,
                    SourceType = SourceTypeEnum.ApplyDisinfectionEquipment,
                    User = user.UserName
                });
            }

            return true;
        }
        private void UpdateHandlingSituations(ApplyDisinfectionEquipmentUpdateStatusViewModel request)
        {
            List<ApplyDisinfectionEquipmentHandlingSituationModel> handlingSituations =
                request.HandlingSituations.Select(e =>
                    new ApplyDisinfectionEquipmentHandlingSituationModel
                    {
                        ApplyId = request.ApplyId,
                        Day = e.Day,
                        Type = e.Type,
                        Name = e.Name,
                        Subsidy = e.Subsidy
                    }).ToList();
            ApplyDisinfectionEquipmentHandlingSituationRepository.DeleteByForeignkey(request.ApplyId);
            ApplyDisinfectionEquipmentHandlingSituationRepository.Create(handlingSituations);
        }
        public bool UpdateApplyDisinfectionEquipmentEpbStatus(ApplyDisinfectionEquipmentUpdateStatusViewModel request)
        {
            UpdateHandlingSituations(request);

            return base.UpdateEpbStatus(request);
        }
        public bool UpdateApplyDisinfectionEquipmentEpaStatus(ApplyDisinfectionEquipmentUpdateStatusViewModel request)
        {
            UpdateHandlingSituations(request);

            return base.UpdateEpaStatus(request);
        }
        public ApplyIndexViewModel<ApplyDisinfectionEquipmentModel> GetApplyIndexViewModel(DutyEnum duty, ApplyRequestViewModel requestViewModel)
        {
            var result = new ApplyIndexViewModel<ApplyDisinfectionEquipmentModel>();
            var filter = GetBaseFilter(duty, requestViewModel);
            result.AddAppliedRequests(ApplyDisinfectionEquipmentRepository.GetByFilter(filter));

            foreach (var ApplyDisinfectionEquipmentModel in result.AppliedRequests)
            {
                MappedViewModel(ApplyDisinfectionEquipmentModel);
            }

            if (string.IsNullOrEmpty(result.ApplyStatus))
            {
                if (result.AppliedRequests.Any())
                {
                    var details = result.AppliedRequests.SelectMany(c => c.Details);
                    var sumList = details.GroupBy(c => c.Item)
                                         .Select(c => new { Item = c.Key, Sum = c.Sum(y => y.Quantity),Days = c.Sum(y=>y.Days) })
                                         .OrderBy(c => c.Item);
                    var sumListString = string.Join("，", sumList.Select(c => $"{c.Item}：{c.Sum} 單位 {c.Days}天"));
                    result.ApplyStatus = $"本次災害已請求：{sumListString}";
                }
                else
                {
                    result.ApplyStatus = "本次災害未請求";
                }
            }

            return result;
        }

        public ApplyDisinfectionEquipmentModel GetCreateModel()
        {
            var returnModel = new ApplyDisinfectionEquipmentModel()
            {
                RequireDate = DateTimeHelper.GetCurrentTime()
            };

            return returnModel;
        }
        public ApplyDisinfectionEquipmentViewModel GetViewModelById(int id)
        {
            var found = ApplyDisinfectionEquipmentRepository.GetByFilter(new ApplyBaseFilterParameter()
            {
                Id = id
            }).FirstOrDefault();

            if (found == null)
            {
                return null;
            }

            return MappedViewModel(found);
        }

        private ApplyDisinfectionEquipmentViewModel MappedViewModel(ApplyDisinfectionEquipmentModel model, bool getFileData = true)
        {
            var viewModel = new ApplyDisinfectionEquipmentViewModel();
            var propertiess = typeof(ApplyDisinfectionEquipmentModel).GetProperties();
            foreach (var property in propertiess)
            {
                var value = property.GetValue(model);
                property.SetValue(viewModel, value);
            }

            var details = ApplyDisinfectionEquipmentRepository.GetDetailsById(viewModel.Id);
            viewModel.AddDetials(details);
            model.AddDetials(details);

            if (getFileData)
            {
                var fileData = FileService.GetBySource(SourceTypeEnum.ApplyDisinfectionEquipment, model.Id)
                                          .FirstOrDefault();
                if (fileData != null)
                {
                    viewModel.FileData = fileData;
                }
            }

            return viewModel;
        }

        public List<ApplyDisinfectionEquipmentViewModel> GetApplyViewModelsByFilter(ApplyBaseFilterParameter filter)
        {
            var applyRequests = ApplyDisinfectionEquipmentRepository.GetByFilter(filter);
            var viewModels = applyRequests.Select(c => MappedViewModel(c, false))
                                          .ToList();
            return viewModels;
        }

        public ApplySupportProcessingDetailViewModel GetApplySupportProcessingDetailViewModel(int id)
        {

            var viewModel = GetViewModelById(id);
            if (viewModel != null)
            {
                var returnViewModel = new ApplySupportProcessingDetailViewModel(viewModel);
                var sumList = viewModel.Details.GroupBy(c => c.Item)
                                               .Select(c => new { Item = c.Key, Sum = c.Sum(y => y.Quantity), Days = c.Sum(y => y.Days) })
                                               .OrderBy(c => c.Item);
                var des = string.Join("，", sumList.Select(c => $"{c.Item}：{c.Sum} 單位  {c.Days}天"));
                returnViewModel.ReqeustDescirptions.Add(des);

                //若 EPBConfirmStatus 是 null 或 pending 要轉為審核中
                if (returnViewModel.EPBConfirmStatus == null || returnViewModel.EPBConfirmStatus == ApplyStatusEnum.Pending)
                {
                    ApplyDisinfectionEquipmentRepository.UpdateEpbConfrimStatusToProcess(returnViewModel.Id);
                    returnViewModel.EPBConfirmStatus = ApplyStatusEnum.Processing;
                    returnViewModel.EPBConfirmUpdateTime = DateTime.Now;
                }

                //若 EPBConfirmStatus 是 null 或 pending 要轉為審核中
                if (returnViewModel.IsToEpa && (returnViewModel.EPAConfirmStatus.HasValue == false || returnViewModel.EPAConfirmStatus.Value == ApplyStatusEnum.Pending))
                {
                    ApplyDisinfectionEquipmentRepository.UpdateEpaConfrimStatusToProcess(returnViewModel.Id);
                    returnViewModel.EPAConfirmStatus = ApplyStatusEnum.Processing;
                    returnViewModel.EPAConfirmUpdateTime = DateTimeHelper.GetCurrentTime();
                }

                return returnViewModel;
            }

            return null;
        }
    }
}