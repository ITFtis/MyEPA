using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyEPA.Models
{
    public class RegistersModel
    {
        [PrimaryKey]
        [Required(ErrorMessage = "帳號不能留白")]
        [StringLength(14, MinimumLength = 1, ErrorMessage = "帳號要1-14個字")]
        public string Id { get; set; }

        [Required(ErrorMessage = "姓名不可留白")]
        [StringLength(14, MinimumLength = 1, ErrorMessage = "姓名要1-14個字")]
        public string Name { get; set; }

        [Required(ErrorMessage = "密碼不能留白")]
        [StringLength(14, MinimumLength = 1, ErrorMessage = "密碼要1-14個字")]
        public string Pwd { get; set; }

        public string VoicePwd { get; set; }

        public string Duty { get; set; }

        public string City { get; set; }

        public string Town { get; set; }

        public string MobilePhone { get; set; }

        public string HumanType { get; set; }

        public string MainContacter { get; set; }

        public string ReportPriority { get; set; }

        public int? CityId { get; set; }
        
        public int? TownId { get; set; }

        public int? DutyId { get; set; }

        public int PositionId { get; set; }

        [DisplayName("辦公室電話")]
        public string OfficePhone { get; set; }
        [DisplayName("電子郵件信箱")]
        public string Email { get; set; }

        [DisplayName("來源IP")]
        public string SourceIP { get; set; }
    }
}