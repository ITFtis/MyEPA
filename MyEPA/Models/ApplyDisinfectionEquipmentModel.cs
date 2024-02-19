using MyEPA.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class ApplyDisinfectionEquipmentModel : ApplyBaseModel
    {
          public List<ApplyDisinfectionEquipmentDetailModel> Details = new List<ApplyDisinfectionEquipmentDetailModel>();

        public void CleanDetails()
        {
            Details.Clear();
        }

        public void AddDetials(List<ApplyDisinfectionEquipmentDetailModel> detailModels)
        {
            Details.AddRange(detailModels);
        }
    }
}