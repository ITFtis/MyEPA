using MyEPA.Enums;
using System.ComponentModel.DataAnnotations;

namespace MyEPA.Models
{
    public class SMSLoginUserNameModel
    {
        [Required]
        public string UserName { get; set; }
        public string ValidateCode { get; set; }
        public string ValidateKey { get; set; }
    }
    public class SMSVerifyLoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public SystemTypeEnum Type { get; set; }
    }
}