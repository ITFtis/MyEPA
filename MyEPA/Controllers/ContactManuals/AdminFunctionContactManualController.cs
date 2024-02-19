using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class AdminFunctionContactManualController : LoginBaseController
    {
        public ActionResult None()
        {
            return PartialView();
        }
        public ActionResult User()
        {
            return PartialView();
        }
        public ActionResult Business()
        {
            return PartialView();
        }
        public ActionResult Administrator()
        {
            return PartialView();
        }
    }
}