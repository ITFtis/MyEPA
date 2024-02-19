using MyEPA.EPA.Attribute;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class CustomServiceController : LoginBaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        
    }
}