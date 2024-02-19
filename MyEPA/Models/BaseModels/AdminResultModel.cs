using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class AdminResultModel
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class AdminResultModel<T> : AdminResultModel
    {
        public T Result { get; set; }
    }
}