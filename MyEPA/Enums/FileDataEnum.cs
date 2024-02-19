using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Enums
{
    public enum FileDataEnum
    {
        /// <summary>
        /// 上傳成功
        /// </summary>
        Success,
        /// <summary>
        /// 檔案大小超過限制
        /// </summary>
        FileSizeOver,
        /// <summary>
        /// 副檔名錯誤
        /// </summary>
        ErrorExtension,
        /// <summary>
        /// 找不到檔案
        /// </summary>
        NotFind,
        /// <summary>
        /// 檔案為空
        /// </summary>
        NoUploadFile,
        /// <summary>
        /// 上傳失敗
        /// </summary>
        Failure,
    }
}