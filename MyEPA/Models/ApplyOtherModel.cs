using MyEPA.Models.BaseModels;
using System.Collections.Generic;

namespace MyEPA.Models
{
    public class ApplyOtherModel : ApplyBaseModel
    {
        public List<ApplyOtherDetailModel> Details = new List<ApplyOtherDetailModel>();

        public void CleanDetails()
        {
            Details.Clear();
        }

        public void AddDetials(List<ApplyOtherDetailModel> detailModels)
        {
            Details.AddRange(detailModels);
        }
    }
}