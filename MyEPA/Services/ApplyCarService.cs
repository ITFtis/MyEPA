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
    /// 請求支援-車輛設備
    /// </summary>
    public class ApplyCarService : ApplyBaseService<ApplyCarModel>, IApplyService<ApplyCarModel, ApplyCarViewModel>
    {
        private readonly ApplyCarRepository ApplyCarRepository = new ApplyCarRepository();
        private readonly ApplyCarHandlingSituationRepository ApplyCarHandlingSituationRepository = new ApplyCarHandlingSituationRepository();
        public bool Create(UserBriefModel user, ApplyCarModel model, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
        {
            SetBasicCreateModel(ref model, user, requestViewModel);

            var id = ApplyCarRepository.Create(model);

            if (file != null)
            {
                FileService.UploadFileByGuidName(new UploadFileBaseModel
                {
                    File = file,
                    SourceId = id,
                    SourceType = SourceTypeEnum.ApplyCar,
                    User = user.UserName
                });
            }

            return true;
        }

        public new List<ApplyCarHandlingSituationModel> GetHandlingSituations(int id)
        {
            return ApplyCarHandlingSituationRepository.GetListByForeignkey(id);
        }
        public bool UpdateApplyCarEpbStatus(ApplyCarUpdateStatusViewModel request)
        {
            UpdateHandlingSituations(request);

            return base.UpdateEpbStatus(request);
        }
        private void UpdateHandlingSituations(ApplyCarUpdateStatusViewModel request)
        {
            List<ApplyCarHandlingSituationModel> handlingSituations =
                request.HandlingSituations.Select(e =>
                    new ApplyCarHandlingSituationModel
                    {
                        ApplyId = request.ApplyId,
                        Day = e.Day,
                        Quantity = e.Quantity,
                        Type = e.Type,
                        CarType = e.CarType,
                        Subsidy = e.Subsidy
                    }).ToList();
            ApplyCarHandlingSituationRepository.DeleteByForeignkey(request.ApplyId);
            ApplyCarHandlingSituationRepository.Create(handlingSituations);
        }
        public bool UpdateApplyCarEpaStatus(ApplyCarUpdateStatusViewModel request)
        {
            UpdateHandlingSituations(request);

            return base.UpdateEpaStatus(request);
        }

        public void Delete(UserBriefModel user, int id)
        {
            var found = ApplyCarRepository.Get(id);
            if (found == null)
            {
                throw new Exception("Data not found");
            }
            ApplyCarRepository.DeleteDetails(id);
            ApplyCarRepository.Delete(id);
        }

        public bool Edit(UserBriefModel user, ApplyCarModel model, HttpPostedFileBase file)
        {
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateUser = user.UserName;

            ApplyCarRepository.Edit(model);

            base.UpdateStatus(user, model);

            if (file != null)
            {
                FileService.DeleteFileBySource(SourceTypeEnum.ApplyCar, model.Id);
                FileService.UploadFileByGuidName(new UploadFileBaseModel
                {
                    File = file,
                    SourceId = model.Id,
                    SourceType = SourceTypeEnum.ApplyCar,
                    User = user.UserName
                });
            }

            return true;
        }
        public ApplyViewModel GetApplyViewModel(DutyEnum duty, ApplyRequestViewModel requestViewModel)
        {
            var filter = GetBaseFilter(duty, requestViewModel);
            var model =
                ApplyCarRepository.GetByFilter(filter).OrderByDescending(e => e.UpdateDate).FirstOrDefault();

            if (model == null)
            {
                return null;
            }

            MappedViewModel(model);

            return new ApplyViewModel
            {
                Quantity = $"{model.Details.Sum(e => e.Quantity)} 輛",
                Status = model.IsToEpa ? model.EPAConfirmStatus.GetValueOrDefault().ToInteger() : model.EPBConfirmStatus.GetValueOrDefault().ToInteger()
            };
        }
        public ApplyIndexViewModel<ApplyCarModel> GetApplyIndexViewModel(DutyEnum duty, ApplyRequestViewModel requestViewModel)
        {
            var result = new ApplyIndexViewModel<ApplyCarModel>();
            var filter = GetBaseFilter(duty, requestViewModel);
            result.AddAppliedRequests(ApplyCarRepository.GetByFilter(filter));

            foreach (var ApplyCarModel in result.AppliedRequests)
            {
                MappedViewModel(ApplyCarModel);
            }

            if (string.IsNullOrEmpty(result.ApplyStatus))
            {
                if (result.AppliedRequests.Count > 0)
                {
                    var details = result.AppliedRequests.SelectMany(c => c.Details);
                    var sumList = details.GroupBy(c => c.ApplyCarTypeId)
                                         .Select(c => new { ApplyCarTypeId = c.Key, Sum = c.Sum(y => y.Quantity) })
                                         .OrderBy(c => c.ApplyCarTypeId);
                    var types = ApplyCarRepository.GetCarTypes();
                    var sumListString = string.Join("，", sumList.Select(c => $"{ types.FirstOrDefault(k => k.Id == c.ApplyCarTypeId)?.DisplayName}： {c.Sum} 輛"));
                    result.ApplyStatus = $"本次災害已請求：{sumListString}";
                }
                else
                {
                    result.ApplyStatus = $"本次災害未請求";

                }

            }

            return result;
        }

        public ApplyCarModel GetCreateModel()
        {
            var returnModel = new ApplyCarModel()
            {
                RequireDate = DateTimeHelper.GetCurrentTime()
            };

            return returnModel;
        }

        public ApplyCarViewModel GetViewModelById(int id)
        {
            var found = ApplyCarRepository.GetByFilter(new ApplyBaseFilterParameter()
            {
                Id = id
            }).FirstOrDefault();

            if (found == null)
            {
                return null;
            }

            return MappedViewModel(found);
        }

        private ApplyCarViewModel MappedViewModel(ApplyCarModel model, bool getFileData = true)
        {
            var viewModel = new ApplyCarViewModel();
            var propertiess = typeof(ApplyCarModel).GetProperties();
            foreach (var property in propertiess)
            {
                var value = property.GetValue(model);
                property.SetValue(viewModel, value);
            }

            var details = ApplyCarRepository.GetDetailsById(viewModel.Id);
            viewModel.AddDetials(details);
            model.AddDetials(details);

            if (getFileData) 
            {
                var fileData = FileService.GetBySource(SourceTypeEnum.ApplyCar, model.Id)
                                        .FirstOrDefault();
                if (fileData != null)
                {
                    viewModel.FileData = fileData;
                }
            }

            return viewModel;
        }

        public List<ApplyCarTypeModel> GetTypes()
        {
            return ApplyCarRepository.GetCarTypes();
        }

        public List<ApplyCarViewModel> GetApplyViewModelsByFilter(ApplyBaseFilterParameter filter)
        {
            var applyRequests = ApplyCarRepository.GetByFilter(filter);
            var viewModels = applyRequests.Select(c => MappedViewModel(c, false))
                                          .ToList();

            //建檔者單位取得
            UsersRepository UsersRepository = new UsersRepository();
            var users = UsersRepository.GetUsersInfoByFilter(new UsersInfoFilterParameter
            {
                UserNames = applyRequests.Select(a => a.CreateUser)
            });
            foreach (var vw in viewModels)
            {
                var u = users.Where(a => a.UserName == vw.CreateUser).FirstOrDefault();
                if (u != null)
                    vw.CreateUserDuty = u.Duty;
            }

            return viewModels;
        }

        public ApplySupportProcessingDetailViewModel GetApplySupportProcessingDetailViewModel(int id)
        {
            var viewModel = GetViewModelById(id);
            if (viewModel != null)
            {
                var returnViewModel = new ApplySupportProcessingDetailViewModel(viewModel);
                var types = ApplyCarRepository.GetCarTypes();
                var sumList = viewModel.Details
                                       .GroupBy(c => c.ApplyCarTypeId)
                                       .Select(c => new { ApplyCarTypeId = c.Key, Sum = c.Sum(y => y.Quantity) })
                                       .OrderBy(c => c.ApplyCarTypeId);
                var des = string.Join("，", sumList.Select(c => $"{ types.FirstOrDefault(k => k.Id == c.ApplyCarTypeId)?.DisplayName}： {c.Sum} 輛"));
                returnViewModel.ReqeustDescirptions.Add(des);

                //若 EPBConfirmStatus 是 null 或 pending 要轉為審核中
                if (returnViewModel.EPBConfirmStatus == null || returnViewModel.EPBConfirmStatus == ApplyStatusEnum.Pending) 
                {
                    ApplyCarRepository.UpdateEpbConfrimStatusToProcess(returnViewModel.Id);
                    returnViewModel.EPBConfirmStatus = ApplyStatusEnum.Processing;
                    returnViewModel.EPBConfirmUpdateTime = DateTimeHelper.GetCurrentTime();
                }

                //若 EPBConfirmStatus 是 null 或 pending 要轉為審核中
                if (returnViewModel.IsToEpa && (returnViewModel.EPAConfirmStatus.HasValue == false || returnViewModel.EPAConfirmStatus.Value == ApplyStatusEnum.Pending))
                {
                    ApplyCarRepository.UpdateEpaConfrimStatusToProcess(returnViewModel.Id);
                    returnViewModel.EPAConfirmStatus = ApplyStatusEnum.Processing;
                    returnViewModel.EPAConfirmUpdateTime = DateTimeHelper.GetCurrentTime();
                }

                return returnViewModel;
            }

            return null;
        }
    }
}