using MyEPA.Models.BaseModels;
using System.Collections.Generic;

namespace MyEPA.Models
{
    public class ApplySubsidyCreateViewModel : ApplyBaseModel
    {
        public List<ApplySubsidyDetailModel> Details { get; set; }

        public ApplySubsidyModel ToModel()
        {
            return new ApplySubsidyModel 
            {
                EPAConfirmDescribe = this.EPAConfirmDescribe,
                EPAConfirmStatus = this.EPAConfirmStatus,
                EPAConfirmUpdateTime = this.EPAConfirmUpdateTime,
                CityId = this.CityId,
                ContactMobilePhone = this.ContactMobilePhone,
                ContactPerson = this.ContactPerson,
                ContactPhone = this.ContactPhone,
                CreateDate = this.CreateDate,
                CreateUser = this.CreateUser,
                Details = this.Details,
                DiasterId = this.DiasterId,
                EPBConfirmDescribe = this.EPBConfirmDescribe,
                EPBConfirmStatus = this.EPBConfirmStatus,
                EPBConfirmUpdateTime = this.EPBConfirmUpdateTime,
                EstimationMethodDescribe = this.EstimationMethodDescribe,
                Id = this.Id,
                IsToEpa = this.IsToEpa,
                PhotoDescribe = this.PhotoDescribe,
                PostStatus = this.PostStatus,
                RequireDate = this.RequireDate,
                Status = this.Status,
                TownId = this.TownId,
                UpdateDate = this.UpdateDate,
                UpdateUser = this.UpdateUser
            };
        }
    }
    public class ApplySubsidyModel : ApplyBaseModel
    {
        public List<ApplySubsidyDetailModel> Details = new List<ApplySubsidyDetailModel>();

        public void CleanDetails()
        {
            Details.Clear();
        }

        public void AddDetials(List<ApplySubsidyDetailModel> detailModels)
        {
            Details.AddRange(detailModels);
        }
    }
}