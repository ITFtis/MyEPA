using System;
using System.ComponentModel;

namespace MyEPA.Models
{

    public class UserGroupModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int Type { get; set; }
        [DisplayName("群組名稱")]

        public string GroupName { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 建立者名稱
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 建立者名稱
        /// </summary>
        public string UpdateUser { get; set; }

    }
}