using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Models.BaseModels;
using MyEPA.ViewModels;
using System.Collections.Generic;
using System.Web;

namespace MyEPA.Services
{
    public interface IApplyService<T, TQ>
           where T : ApplyBaseModel
           where TQ : ApplyBaseModel
    {
        bool Create(UserBriefModel user, T model, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel);
        void Delete(UserBriefModel user, int id);
        bool Edit(UserBriefModel user, T model, HttpPostedFileBase file);
        ApplyIndexViewModel<T> GetApplyIndexViewModel(DutyEnum duty, ApplyRequestViewModel reqeustViewModel);
        T GetCreateModel();
        TQ GetViewModelById(int id);
        ApplySupportProcessingDetailViewModel GetApplySupportProcessingDetailViewModel(int id);
        bool UpdateEpbStatus(ApplySupportUpdateStatusViewModel request);
        bool UpdateEpaStatus(ApplySupportUpdateStatusViewModel request);
        T Get(ApplyRequestViewModel request);

        ApplyViewModel GetApplyViewModel(DutyEnum duty, ApplyRequestViewModel requestViewModel);
        bool UpdateEpaStatus(ApplySupportUpdateStatusViewModel request, List<ApplyHandlingSituationViewModel> handlingSituations);
        bool UpdateEpbStatus(ApplySupportUpdateStatusViewModel request, List<ApplyHandlingSituationViewModel> handlingSituations);
        List<ApplyHandlingSituationModel> GetHandlingSituations(int id);
    }
}