using System;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class FileDataModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int SourceId { get; set; }
        /// <summary>
        /// 檔案來源 
        /// SourceTypeEnum.cs
        /// </summary>
        public int SourceType { get; set; }
        /// <summary>
        /// 真實檔案名稱
        /// </summary>
        public string RealFileName { get; set; }
        /// <summary>
        /// 使用者檔案名稱
        /// </summary>
        [DisplayName("檔案名稱")]
        public string UserFileName { get; set; }
        [DisplayName("上傳時間")]
        public DateTime CreateDate { get; set; }
        [DisplayName("上傳人")]
        public string CreateUser { get; set; }
        [DisplayName("相關描述")]
        public string Description { get; set; }

    }
}