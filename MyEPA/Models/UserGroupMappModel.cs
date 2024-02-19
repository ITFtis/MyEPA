using System;

namespace MyEPA.Models
{
    public class UserGroupMappModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int GroupId { get; set; }

        public int UserId { get; set; }
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
    public class UserGroupMappBriefModel
    {

        public int GroupId { get; set; }

        public int UserId { get; set; }
    }
    
}