using MyEPA.Models;
using MyEPA.Models.BaseModels;
using System.Collections.Generic;

namespace MyEPA.ViewModels
{
    public class ApplyIndexViewModel<T> where T : ApplyBaseModel
    {
        /// <summary>
        /// 此次災害請求情形
        /// </summary>
        public string ApplyStatus { get; set; }

        /// <summary>
        /// 已請求情形
        /// </summary>
        public List<T> AppliedRequests { get; private set; } = new List<T>();

        public void AddAppliedRequests(List<T> applyRequests)
        {
            AppliedRequests.AddRange(applyRequests);
        }

        public void ClearAppliedRequests()
        {
            AppliedRequests.Clear();
        }
    }

    public class ApplyViewModel
    {
        public string Quantity { get; set; }
        public int Status { get; set; }
    }
}