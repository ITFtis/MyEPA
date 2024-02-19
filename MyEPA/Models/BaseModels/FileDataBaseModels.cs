using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class FileDataBaseModels
    {
        /// <summary>
        /// 使用者上傳的檔案名稱
        /// </summary>
        public string UserFileName { get; set; }

        public string UserFileNameAndExtension
        {
            get
            {
                return $"{UserFileName}{Extension}";
            }
        }
        /// <summary>
        /// 檔案大小
        /// </summary>
        public int FileLength { get; set; }
        /// <summary>
        /// 檔案位元組
        /// </summary>
        public byte[] FileByte { get; set; }
        /// <summary>
        /// 副檔名
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// 真實檔案名稱
        /// </summary>
        public string RealFileName { get; set; }

        public string RealFileNameAndExtension 
        { 
            get 
            {
                return $"{RealFileName}{Extension}";
            } 
        }

        public string ServerMapPath { get; set; }
    }
}