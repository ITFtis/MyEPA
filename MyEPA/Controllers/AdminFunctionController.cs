using MyEPA.Enums;
using MyEPA.Services;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class AdminFunctionController : LoginBaseController
    {
        public ActionResult Cleaning()
        {
            ViewBag.IsAdmin = GetIsAdmin();

            return PartialView();
        }
        public ActionResult EPA()
        {                                    
            ViewBag.IsAdmin = GetIsAdmin();

            return PartialView();
        }
        public ActionResult EPB()
        {
            ViewBag.IsAdmin = GetIsAdmin();

            return PartialView();
        }
        public ActionResult Water()
        {
            ViewBag.IsAdmin = GetIsAdmin();

            return PartialView();
        }
        public ActionResult Team()
        {
            ViewBag.IsAdmin = GetIsAdmin();

            return PartialView();
        }
        public ActionResult Corps()
        {
            ViewBag.IsAdmin = GetIsAdmin();

            return PartialView();
        }
        
    }
}