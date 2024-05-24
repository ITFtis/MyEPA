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
    /// 請求支援-藥品
    /// </summary>
    public class ApplyMedicineService :ApplyBaseService<ApplyMedicineModel>, IApplyService<ApplyMedicineModel, ApplyMedicineViewModel>
    {
        private readonly ApplyMedicineRepository ApplyMedicineRepository = new ApplyMedicineRepository();
        private readonly ApplyMedicineHandlingSituationRepository ApplyMedicineHandlingSituationRepository = new ApplyMedicineHandlingSituationRepository();
        public bool Create(UserBriefModel user, ApplyMedicineModel model, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
        {
            SetBasicCreateModel(ref model, user, requestViewModel);
            var id = ApplyMedicineRepository.Create(model);

            if (file != null)
            {
                FileService.UploadFileByGuidName(new UploadFileBaseModel
                {
                    File = file,
                    SourceId = id,
                    SourceType = SourceTypeEnum.ApplyMedicine,
                    User = user.UserName
                });
            }

            return true;
        }
        public new List<ApplyMedicineHandlingSituationModel> GetHandlingSituations(int id)
        {
            return ApplyMedicineHandlingSituationRepository.GetListByForeignkey(id);
        }
        public ApplyViewModel GetApplyViewModel(DutyEnum duty, ApplyRequestViewModel requestViewModel)
        {
            var filter = GetBaseFilter(duty, requestViewModel);
            var model =
                ApplyMedicineRepository.GetByFilter(filter).OrderByDescending(e => e.UpdateDate).FirstOrDefault();

            if (model == null)
            {
                return null;
            }

            MappedViewModel(model);

            return new ApplyViewModel
            {
                Quantity = $"{model.Details.Sum(e => e.Quantity)} 公升",
                Status = model.IsToEpa ? model.EPBConfirmStatus.GetValueOrDefault().ToInteger() : model.EPAConfirmStatus.GetValueOrDefault().ToInteger()
            };
        }

        public bool UpdateApplyMedicineEpaStatus(ApplyMedicineUpdateStatusViewModel request)
        {
            UpdateHandlingSituations(request);

            return base.UpdateEpaStatus(request);
        }
        public bool UpdateApplyMedicineEpbStatus(ApplyMedicineUpdateStatusViewModel request)
        {
            UpdateHandlingSituations(request);

            return base.UpdateEpbStatus(request);
        }
        private void UpdateHandlingSituations(ApplyMedicineUpdateStatusViewModel request)
        {
            List<ApplyMedicineHandlingSituationModel> handlingSituations =
                request.HandlingSituations.Select(e =>
                    new ApplyMedicineHandlingSituationModel
                    {
                        ApplyId = request.ApplyId,
                        Quantity = e.Quantity,
                        Type = e.Type,
                        MedicineType = e.MedicineType,
                        Subsidy = e.Subsidy
                    }).ToList();
            ApplyMedicineHandlingSituationRepository.DeleteByForeignkey(request.ApplyId);
            ApplyMedicineHandlingSituationRepository.Create(handlingSituations);
        }

        public void Delete(UserBriefModel user, int id)
        {
            var found = ApplyMedicineRepository.Get(id);
            if (found == null) 
            {
                throw new Exception("Data not found");
            }
            ApplyMedicineRepository.DeleteDetails(id);
            ApplyMedicineRepository.Delete(id);
        }

        public bool Edit(UserBriefModel user, ApplyMedicineModel model, HttpPostedFileBase file)
        {
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateUser = user.UserName;

            ApplyMedicineRepository.Edit(model);

            base.UpdateStatus(user, model);

            if (file != null)
            {
                FileService.DeleteFileBySource(SourceTypeEnum.ApplyMedicine, model.Id);
                FileService.UploadFileByGuidName(new UploadFileBaseModel
                {
                    File = file,
                    SourceId = model.Id,
                    SourceType = SourceTypeEnum.ApplyMedicine,
                    User = user.UserName
                });
            }

            return true;
        }

        public ApplyIndexViewModel<ApplyMedicineModel> GetApplyIndexViewModel(DutyEnum duty, ApplyRequestViewModel requestViewModel)
        {
            var result = new ApplyIndexViewModel<ApplyMedicineModel>();
            var filter = GetBaseFilter(duty, requestViewModel);
            result.AddAppliedRequests(ApplyMedicineRepository.GetByFilter(filter));

            foreach (var applyMedicineModel in result.AppliedRequests)
            {
                MappedViewModel(applyMedicineModel);
            }

            if (string.IsNullOrEmpty(result.ApplyStatus))
            {
                var details = result.AppliedRequests.SelectMany(c => c.Details);
                var sumList = details.GroupBy(c => c.MedicineType)
                                     .Select(c => new { MedicineType = c.Key, Sum = c.Sum(y => y.Quantity) })
                                     .OrderBy(c=> c.MedicineType.ToInteger());
                var sumListString = string.Join("，", sumList.Select(c => $"{c.MedicineType.GetDescription()}：{c.Sum}公升"));
                result.ApplyStatus = $"本次災害已請求：{sumListString}";
            }

            return result;
        }

        public ApplyMedicineModel GetCreateModel()
        {
            var returnModel = new ApplyMedicineModel()
            {
                RequireDate = DateTimeHelper.GetCurrentTime()
            };

            return returnModel;
        }

        public ApplyMedicineViewModel GetViewModelById(int id)
        {
            var found = ApplyMedicineRepository.GetByFilter(new ApplyBaseFilterParameter()
            {
                Id = id
            }).FirstOrDefault();

            if (found == null)
            {
                return null;
            }

            return MappedViewModel(found);
        }

        private ApplyMedicineViewModel MappedViewModel(ApplyMedicineModel model, bool getFiledata = true)
        {
            var viewModel = new ApplyMedicineViewModel();
            var propertiess = typeof(ApplyMedicineModel).GetProperties();
            foreach (var property in propertiess)
            {
                var value = property.GetValue(model);
                property.SetValue(viewModel, value);
            }

            var details = ApplyMedicineRepository.GetDetailsById(viewModel.Id);
            viewModel.AddDetials(details);
            model.AddDetials(details);

            if (getFiledata) 
            {
                var fileData = FileService.GetBySource(SourceTypeEnum.ApplyMedicine, model.Id)
                                          .FirstOrDefault();
                if (fileData != null)
                {
                    viewModel.FileData = fileData;
                }
            }

            return viewModel;
        }

        public List<ApplyMedicineViewModel> GetApplyViewModelsByFilter(ApplyBaseFilterParameter filter)
        {
            var applyRequests = ApplyMedicineRepository.GetByFilter(filter);
            var viewModels = applyRequests.Select(c => MappedViewModel(c,false))
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
                                       .GroupBy(c => c.MedicineType)
                                       .Select(c => new { MedicineType = c.Key, Sum = c.Sum(y => y.Quantity) })
                                       .OrderBy(c => c.MedicineType.ToInteger());
                var des = string.Join("，", sumList.Select(c => $"{c.MedicineType.GetDescription()}：{c.Sum}單位"));
                returnViewModel.ReqeustDescirptions.Add(des);
                
                //若 EPBConfirmStatus 是 null 或 pending 要轉為審核中
                if (returnViewModel.EPBConfirmStatus == null || returnViewModel.EPBConfirmStatus == ApplyStatusEnum.Pending)
                {
                    ApplyMedicineRepository.UpdateEpbConfrimStatusToProcess(returnViewModel.Id);
                    returnViewModel.EPBConfirmStatus = ApplyStatusEnum.Processing;
                    returnViewModel.EPBConfirmUpdateTime = DateTime.Now;
                }

                //若 EPBConfirmStatus 是 null 或 pending 要轉為審核中
                if (returnViewModel.IsToEpa && (returnViewModel.EPAConfirmStatus.HasValue == false || returnViewModel.EPAConfirmStatus.Value == ApplyStatusEnum.Pending))
                {
                    ApplyMedicineRepository.UpdateEpaConfrimStatusToProcess(returnViewModel.Id);
                    returnViewModel.EPAConfirmStatus = ApplyStatusEnum.Processing;
                    returnViewModel.EPAConfirmUpdateTime = DateTimeHelper.GetCurrentTime();
                }

                return returnViewModel;
            }

            return null;
        }
    }
}