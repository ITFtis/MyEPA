using System.ComponentModel;

namespace MyEPA.Models
{
    public class ContactManualDownloadRecordModel : BaseCreateModel
    {
        [AutoKey]
        public int Id { get; set; }
        public int UserId { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("手機")]
        public string MobilePhone { get; set; }
    }
}