using MyEPA.Models.BaseModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyEPA.Models
{
    //申請人力支援用 Model
    public class ApplyPeopleModel: ApplyBaseModel
    {
        /// <summary>
        /// 清潔隊人數
        /// </summary>
        [DisplayName("清潔隊需要人數")]
        [Required(ErrorMessage = "請填入需要人數")]
        [DefaultValue(0)]
        [Range(0, int.MaxValue)]
        public int CleaningMemberQuantity { get; set; }


        /// <summary>
        /// 清潔隊需要天數
        /// </summary>
        [DisplayName("清潔隊需要天數")]
        [Required(ErrorMessage = "請填入需要天數")]
        [DefaultValue(0)]
        [Range(0, int.MaxValue)]
        public int CleaningMemberDays { get; set; }

        /// <summary>
        /// 國軍需要人數
        /// </summary>
        [DisplayName("國軍需要人數")]
        [Required(ErrorMessage = "請填入需要人數")]
        [DefaultValue(0)]
        [Range(0, int.MaxValue)]
        public int NationalArmyQuantity { get; set; }

        /// <summary>
        /// 國軍需要天數
        /// </summary>
        [DisplayName("國軍需要天數")]
        [Required(ErrorMessage = "請填入需要天數")]
        [DefaultValue(0)]
        [Range(0, int.MaxValue)]
        public int NationalArmyDays { get; set; }
    }
}