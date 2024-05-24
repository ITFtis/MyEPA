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
    /// 請求支援-其他
    /// </summary>
    public class ApplyOtherService :ApplyBaseService<ApplyOtherModel>, IApplyService<ApplyOtherModel, ApplyOtherViewModel>
    {
        private readonly ApplyOtherRepository ApplyOtherRepository = new ApplyOtherRepository();
        public bool Create(UserBriefModel user, ApplyOtherModel model, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
        {
            SetBasicCreateModel(ref model, user, requestViewModel);

            var id = ApplyOtherRepository.Create(model);

            if (file != null)
            {
                FileService.UploadFileByGuidName(new UploadFileBaseModel
                {
                    File = file,
                    SourceId = id,
                    SourceType = SourceTypeEnum.ApplyOther,
                    User = user.UserName
                });
            }

            return true;
        }
        public ApplyViewModel GetApplyViewModel(DutyEnum duty, ApplyRequestViewModel requestViewModel)
        {
            var filter = GetBaseFilter(duty, requestViewModel);
            var model =
                ApplyOtherRepository.GetByFilter(filter).OrderByDescending(e => e.UpdateDate).FirstOrDefault();

            if(model == null)
            {
                return null;
            }

            MappedViewModel(model);

            return new ApplyViewModel
            {
                Quantity = $"{model.Details.Sum(e => e.Quantity)} {model.Details.Select(e=>e.Unit).FirstOrDefault()}",
                Status = model.IsToEpa ? model.EPBConfirmStatus.GetValueOrDefault().ToInteger() : model.EPAConfirmStatus.GetValueOrDefault().ToInteger()
            };
        }
        public void Delete(UserBriefModel user, int id)
        {
            var found = ApplyOtherRepository.Get(id);
            if (found == null) 
            {
                throw new Exception("Data not found");
            }
            ApplyOtherRepository.DeleteDetails(id);
            ApplyOtherRepository.Delete(id);
        }

        public bool Edit(UserBriefModel user, ApplyOtherModel model, HttpPostedFileBase file)
        {
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateUser = user.UserName;

            ApplyOtherRepository.Edit(model);

            base.UpdateStatus(user, model);

            if (file != null)
            {
                FileService.DeleteFileBySource(SourceTypeEnum.ApplyOther, model.Id);
                FileService.UploadFileByGuidName(new UploadFileBaseModel
                {
                    File = file,
                    SourceId = model.Id,
                    SourceType = SourceTypeEnum.ApplyOther,
                    User = user.UserName
                });
            }

            return true;
        }

        public ApplyIndexViewModel<ApplyOtherModel> GetApplyIndexViewModel(DutyEnum duty, ApplyRequestViewModel requestViewModel)
        {
            var result = new ApplyIndexViewModel<ApplyOtherModel>();
            var filter = GetBaseFilter(duty, requestViewModel);

            result.AddAppliedRequests(ApplyOtherRepository.GetByFilter(filter));

            foreach (var ApplyOtherModel in result.AppliedRequests) 
            {
                MappedViewModel(ApplyOtherModel);
            }

            if (string.IsNullOrWhiteSpace(result.ApplyStatus))
            {
                if (result.AppliedRequests.Count > 0)
                {
                    var details = result.AppliedRequests.SelectMany(c => c.Details);
                    //同名稱+單位視為一種
                    var sumList = details.GroupBy(c => new { c.Item, c.Unit })
                                         .Select(c => new { c.Key.Item, c.Key.Unit, Sum = c.Sum(y => y.Quantity) })
                                         .ToList();
                    var sumListString = string.Join("，", sumList.Select(c => $"{c.Item}：{c.Sum} {c.Unit}"));
                    result.ApplyStatus = $"本次災害已請求：{sumListString}";
                }
                else
                {
                    result.ApplyStatus = "本次災害未請求";
                }
            }

            return result;
        }

        public ApplyOtherModel GetCreateModel()
        {
            var returnModel = new ApplyOtherModel()
            {
                RequireDate = DateTimeHelper.GetCurrentTime()
            };

            return returnModel;
        }

        public ApplyOtherViewModel GetViewModelById(int id)
        {
            var found = ApplyOtherRepository.GetByFilter(new ApplyBaseFilterParameter()
            {
                Id = id
            }).FirstOrDefault();

            if (found == null)
            {
                return null;
            }

            return MappedViewModel(found);
        }

        private ApplyOtherViewModel MappedViewModel(ApplyOtherModel model, bool getFiledata = true)
        {
            var viewModel = new ApplyOtherViewModel();
            var propertiess = typeof(ApplyOtherModel).GetProperties();
            foreach (var property in propertiess)
            {
                var value = property.GetValue(model);
                property.SetValue(viewModel, value);
            }

            var details = ApplyOtherRepository.GetDetailsById(viewModel.Id);
            viewModel.AddDetials(details);
            model.AddDetials(details);

            if (getFiledata)
            {
                var fileData = FileService.GetBySource(SourceTypeEnum.ApplyOther, model.Id)
                                          .FirstOrDefault();
                if (fileData != null)
                {
                    viewModel.FileData = fileData;
                }
            }

            return viewModel;
        }

        public List<ApplyOtherViewModel> GetApplyViewModelsByFilter(ApplyBaseFilterParameter filter)
        {
            var applyRequests = ApplyOtherRepository.GetByFilter(filter);
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
                //同名稱+單位視為一種
                var sumList = viewModel.Details.GroupBy(c => new { c.Item, c.Unit })
                                     .Select(c => new { c.Key.Item, c.Key.Unit, Sum = c.Sum(y => y.Quantity) })
                                     .ToList();
                var des = string.Join("，", sumList.Select(c => $"{c.Item}：{c.Sum} {c.Unit}"));
                returnViewModel.ReqeustDescirptions.Add(des);


                //若 EPBConfirmStatus 是 null 或 pending 要轉為審核中
                if (returnViewModel.EPBConfirmStatus == null || returnViewModel.EPBConfirmStatus == ApplyStatusEnum.Pending)
                {
                    ApplyOtherRepository.UpdateEpbConfrimStatusToProcess(returnViewModel.Id);
                    returnViewModel.EPBConfirmStatus = ApplyStatusEnum.Processing;
                    returnViewModel.EPBConfirmUpdateTime = DateTime.Now;
                }

                //若 EPBConfirmStatus 是 null 或 pending 要轉為審核中
                if (returnViewModel.IsToEpa && (returnViewModel.EPAConfirmStatus.HasValue == false || returnViewModel.EPAConfirmStatus.Value == ApplyStatusEnum.Pending))
                {
                    ApplyOtherRepository.UpdateEpaConfrimStatusToProcess(returnViewModel.Id);
                    returnViewModel.EPAConfirmStatus = ApplyStatusEnum.Processing;
                    returnViewModel.EPAConfirmUpdateTime = DateTimeHelper.GetCurrentTime();
                }

                return returnViewModel;
            }

            return null;
        }

    }
}