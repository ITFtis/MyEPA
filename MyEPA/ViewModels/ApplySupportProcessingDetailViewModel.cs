using MyEPA.Models;
using MyEPA.Models.BaseModels;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.ViewModels
{
    /// <summary>
    /// 審核用
    /// </summary>
    public class ApplySupportProcessingDetailViewModel: ApplyBaseModel
    {
        public ApplySupportProcessingDetailViewModel() { }
        public ApplySupportProcessingDetailViewModel(ApplyBaseModel baseModel)
        {
            var properties = baseModel.GetType()
                                      .GetProperties();
            var selfProperties = typeof(ApplySupportProcessingDetailViewModel).GetProperties();

            foreach (var propery in properties)
            {
                var value = propery.GetValue(baseModel);
                var foundProperty = selfProperties.FirstOrDefault(c => c.Name == propery.Name);
                foundProperty?.SetValue(this, value);
            }
        }

        public FileDataModel FileData { get; set; }

        public string CityName { get; set; }

        public string TownName { get; set; }

        public List<string> ReqeustDescirptions { get; } = new List<string>();
    }
}