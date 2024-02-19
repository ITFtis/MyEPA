using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MyEPA.Models
{
    [Table("Landfill")]
    public class DeleteLandfillModel
    {
        [AutoKey]
        public string Id { get; set; }
        public string ContactUnit { get; set; }
        public string Xpos { get; set; }
        public string Ypos { get; set; }
        public string City { get; set; }
    }
}