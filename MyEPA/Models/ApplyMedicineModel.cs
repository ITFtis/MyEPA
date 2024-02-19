using MyEPA.Models.BaseModels;
using System.Collections.Generic;

namespace MyEPA.Models
{
    public class ApplyMedicineModel : ApplyBaseModel
    {
        public List<ApplyMedicineDetailModel> Details  = new List<ApplyMedicineDetailModel>();

        public void CleanDetails() 
        {
            Details.Clear();
        }

        public void AddDetials(List<ApplyMedicineDetailModel> detailModels) 
        {
            Details.AddRange(detailModels);
        }
    }
}