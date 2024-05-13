using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers.Rec
{
    public class TestMailController : LoginBaseController
    {
        // GET: TestMail
        public ActionResult Index()
        {
            bool isAdmin = GetIsAdmin();

            //限定管理者使用
            if (!isAdmin)
            {                
                return RedirectToAction("LoginRedirect", "Home", new { });
            }

            return View();
        }
    }
}