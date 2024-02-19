using MyEPA.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MyEPA.ViewModels
{
    public class ContactManualFileDataUploadViewModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("檔案")]
        public HttpPostedFileBase File { get; set; }
        [DisplayName("SourceType")]
        public SourceTypeEnum SourceType { get; set; }
        [DisplayName("描述")]
        public string Description { get; set; }
        [Required]
        [DisplayName("部門")]
        public int? DepartmentId { get; set; }
        public string UserFileName { get; set; }
        public string RealFileName { get; set; }
    }
    public class ContactManualSupervisionFileDataUploadViewModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("檔案")]
        public HttpPostedFileBase File { get; set; }
        [DisplayName("SourceType")]
        public SourceTypeEnum SourceType { get; set; }
        [DisplayName("描述")]
        public string Description { get; set; }
        [Required]
        [DisplayName("章節")]
        public int? SourceId { get; set; }
        public string UserFileName { get; set; }
        public string RealFileName { get; set; }
    }
}