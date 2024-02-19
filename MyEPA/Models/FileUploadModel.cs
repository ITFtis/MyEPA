using System;

namespace MyEPA.Models
{
    public class FileUploadModel
    {
        [AutoKey]
        public int ID { get; set; }

        public string FILENAME { get; set; }

        public string FILETEXT { get; set; }

        public string DOWNADMIN { get; set; }

        public int? USERLV { get; set; }

        public string USERID { get; set; }

        public DateTime? UPTIME { get; set; }

    }
}