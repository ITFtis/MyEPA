using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEPA.Models.Deds
{
    [Table("User")]
    public class User
    {
        [Key]
        [Display(Name = "帳號")]
        public virtual string Id { get; set; }

        [Display(Name = "密碼")]
        public virtual string Password { get; set; }
    }
}