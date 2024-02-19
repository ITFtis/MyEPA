using MyEPA.Enums;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class ContactManualModel : BaseModel
    {
        [AutoKey]
        public int Id { get; set; }

        public ContactManualTypeEnum Type { get; set; }
        /// <summary>
        /// CityId,DepartmentId
        /// </summary>
        public int SourceId { get; set; }
        [DisplayName("人員")]
        public int UserId { get; set; }
        public int Sort { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public int RoleId { get; set; }
    }
}