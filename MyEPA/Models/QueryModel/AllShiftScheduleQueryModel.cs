using System.ComponentModel;

namespace MyEPA.Models
{
    public class AllShiftScheduleQueryModel
    {
        [DisplayName("班別")]
        public bool IsNight { get; set; }
        public int DepartmentId { get; set; }
        [DisplayName("處室")]
        public string DepartmentName { get; set; }

        [DisplayName("進駐人員")]
        public string Name { get; set; }
        [DisplayName("行動電話")]
        public string MobilePhone { get; set; }

    }
}