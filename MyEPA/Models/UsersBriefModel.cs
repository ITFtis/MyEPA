using MyEPA.Enums;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class UsersBriefModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }

        [DisplayName("縣市")]
        public string City { get; set; }
        [DisplayName("鄉鎮")]
        public string Town { get; set; }
        [DisplayName("行動電話")]
        public string MobilePhone { get; set; }
        public ContactManualDutyEnum ContactManualDuty { get; set; }
    }
}