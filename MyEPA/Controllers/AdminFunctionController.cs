using MyEPA.Enums;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class AdminFunctionController : LoginBaseController
    {
        public ActionResult Cleaning()
        {
            return PartialView();
        }
        public ActionResult EPA()
        {
            return PartialView();
        }
        public ActionResult EPB()
        {
            return PartialView();
        }
        public ActionResult Water()
        {
            return PartialView();
        }
        public ActionResult Team()
        {
            return PartialView();
        }
        public ActionResult Corps()
        {
            return PartialView();
        }
        
    }
}