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
    /// 請求支援-補助款
    /// </summary>
    public class ApplySubsidyService : ApplyBaseService<ApplySubsidyModel>, IApplyService<ApplySubsidyModel, ApplySubsidyViewModel>
    {
        private readonly ApplySubsidyRepository ApplySubsidyRepository = new ApplySubsidyRepository();
        public ApplyViewModel GetApplyViewModel(DutyEnum duty, ApplyRequestViewModel requestViewModel)
        {
            var filter = GetBaseFilter(duty, requestViewModel);
            var model =
                ApplySubsidyRepository.GetByFilter(filter).OrderByDescending(e => e.UpdateDate).FirstOrDefault();

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
        public bool Create(UserBriefModel user, ApplySubsidyModel model, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
        {
            SetBasicCreateModel(ref model, user, requestViewModel);

            var id = ApplySubsidyRepository.Create(model);

            if (file != null)
            {
                FileService.UploadFileByGuidName(new UploadFileBaseModel
                {
                    File = file,
                    SourceId = id,
                    SourceType = SourceTypeEnum.ApplySubsidy,
                    User = user.UserName
                });
            }

            return true;
        }

        public void Delete(UserBriefModel user, int id)
        {
            var found = ApplySubsidyRepository.Get(id);
            if (found == null)
            {
                throw new Exception("Data not found");
            }
            ApplySubsidyRepository.DeleteDetails(id);
            ApplySubsidyRepository.Delete(id);
        }

        public bool Edit(UserBriefModel user, ApplySubsidyModel model, HttpPostedFileBase file)
        {
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateUser = user.UserName;

            ApplySubsidyRepository.Edit(model);

            base.UpdateStatus(user, model);

            if (file != null)
            {
                FileService.DeleteFileBySource(SourceTypeEnum.ApplySubsidy, model.Id);
                FileService.UploadFileByGuidName(new UploadFileBaseModel
                {
                    File = file,
                    SourceId = model.Id,
                    SourceType = SourceTypeEnum.ApplySubsidy,
                    User = user.UserName
                });
            }

            return true;
        }

        public ApplyIndexViewModel<ApplySubsidyModel> GetApplyIndexViewModel(DutyEnum duty, ApplyRequestViewModel requestViewModel)
        {
            var result = new ApplyIndexViewModel<ApplySubsidyModel>();
            var filter = GetBaseFilter(duty, requestViewModel);
            result.AddAppliedRequests(ApplySubsidyRepository.GetByFilter(filter));

            foreach (var ApplySubsidyModel in result.AppliedRequests)
            {
                MappedViewModel(ApplySubsidyModel);
            }

            if (string.IsNullOrEmpty(result.ApplyStatus))
            {
                var details = result.AppliedRequests.SelectMany(c => c.Details);
                var sumList = details.GroupBy(c => c.SubsidyType)
                                     .Select(c => new { SubsidyType = c.Key, Sum = c.Sum(y => y.Quantity) })
                                     .OrderBy(c => c.SubsidyType.ToInteger());
                var sumListString = string.Join("，", sumList.Select(c => $"{c.SubsidyType.GetDescription()}：{c.Sum}單位"));
                result.ApplyStatus = $"本次災害已請求：{sumListString}";
            }

            return result;
        }

        public ApplySubsidyModel GetCreateModel()
        {
            var returnModel = new ApplySubsidyModel()
            {
                RequireDate = DateTimeHelper.GetCurrentTime()
            };

            return returnModel;
        }

        public ApplySubsidyViewModel GetViewModelById(int id)
        {
            var found = ApplySubsidyRepository.GetByFilter(new ApplyBaseFilterParameter()
            {
                Id = id
            }).FirstOrDefault();

            if (found == null)
            {
                return null;
            }

            return MappedViewModel(found);
        }

        private ApplySubsidyViewModel MappedViewModel(ApplySubsidyModel model, bool getFiledata = true)
        {
            var viewModel = new ApplySubsidyViewModel();
            var propertiess = typeof(ApplySubsidyModel).GetProperties();
            foreach (var property in propertiess)
            {
                var value = property.GetValue(model);
                property.SetValue(viewModel, value);
            }

            var details = ApplySubsidyRepository.GetDetailsById(viewModel.Id);
            viewModel.AddDetials(details);
            model.AddDetials(details);

            if (getFiledata)
            {
                var fileData = FileService.GetBySource(SourceTypeEnum.ApplySubsidy, model.Id)
                                          .FirstOrDefault();
                if (fileData != null)
                {
                    viewModel.FileData = fileData;
                }
            }

            return viewModel;
        }

        public List<ApplySubsidyTypeDetailModel> GetTypeDetails()
        {
            return ApplySubsidyRepository.GetTypeDetails();
        }

        public List<ApplySubsidyTypeDetailModel> GetTypeDetails(int typeId)
        {
            return ApplySubsidyRepository.GetTypeDetails(typeId);
        }

        public List<ApplySubsidyViewModel> GetApplyViewModelsByFilter(ApplyBaseFilterParameter filter)
        {
            var applyRequests = ApplySubsidyRepository.GetByFilter(filter);
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
                var sumList = viewModel.Details
                                       .GroupBy(c => c.SubsidyType)
                                       .Select(c => new
                                       {
                                           SubsidyType = c.Key,
                                           Sum = c.Sum(y => y.Quantity),
                                           Days = c.Sum(y => y.NeedDays),
                                           Price = c.Sum(y => y.Price)
                                       })
                                       .OrderBy(c => c.SubsidyType.ToInteger());
                var des = string.Join("，", sumList.Select(c => $"{c.SubsidyType.GetDescription()}：項目金額 {c.Price}，請求數量 {c.Sum}，請求天數 {c.Days}天"));
                returnViewModel.ReqeustDescirptions.Add(des);

                //若 EPBConfirmStatus 是 null 或 pending 要轉為審核中
                if (returnViewModel.EPBConfirmStatus == null || returnViewModel.EPBConfirmStatus == ApplyStatusEnum.Pending)
                {
                    ApplySubsidyRepository.UpdateEpbConfrimStatusToProcess(returnViewModel.Id);
                    returnViewModel.EPBConfirmStatus = ApplyStatusEnum.Processing;
                    returnViewModel.EPBConfirmUpdateTime = DateTime.Now;
                }

                //若 EPBConfirmStatus 是 null 或 pending 要轉為審核中
                if (returnViewModel.IsToEpa && (returnViewModel.EPAConfirmStatus.HasValue == false || returnViewModel.EPAConfirmStatus.Value == ApplyStatusEnum.Pending))
                {
                    ApplySubsidyRepository.UpdateEpaConfrimStatusToProcess(returnViewModel.Id);
                    returnViewModel.EPAConfirmStatus = ApplyStatusEnum.Processing;
                    returnViewModel.EPAConfirmUpdateTime = DateTimeHelper.GetCurrentTime();
                }

                return returnViewModel;
            }

            return null;
        }
    }
}