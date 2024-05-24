using MyEPA.Models.BaseModels;
using System.Collections.Generic;

namespace MyEPA.Models
{
    public class ApplyCarModel : ApplyBaseModel
    {
        public List<ApplyCarDetailModel> Details = new List<ApplyCarDetailModel>();

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