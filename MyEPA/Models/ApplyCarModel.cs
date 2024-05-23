using MyEPA.Models.BaseModels;
using System.Collections.Generic;

namespace MyEPA.Models
{
    public class ApplyCarModel : ApplyBaseModel
    {
        public List<ApplyCarDetailModel> Details = new List<ApplyCarDetailModel>();

        /// <summary>
        /// 建立者單位
        /// </summary>
        public string CreateUserDuty { get; set; }

        public void CleanDetails()
        {
            Details.Clear();
        }

        public void AddDetials(List<ApplyCarDetailModel> detailModels)
        {
            Details.AddRange(detailModels);
        }
    }
}