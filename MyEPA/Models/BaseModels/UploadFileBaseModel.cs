using MyEPA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class UploadFileBaseModel
    {
        public HttpPostedFileBase File { get; set; }
        public SourceTypeEnum SourceType { get; set; }
        public int SourceId { get; set; }
        public string User { get; set; }
        public string Description { get; set; }

    }
}