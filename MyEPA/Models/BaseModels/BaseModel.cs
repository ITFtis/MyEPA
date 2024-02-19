using System;

namespace MyEPA.Models
{
    public class BaseModel : BaseCreateModel
    {
        public DateTime UpdateDate { get; set; }

        public string UpdateUser { get; set; }
    }
}