using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers.Rec
{
    public class OverValidLoginController : LoginBaseController
    {
        UsersService UsersService = new UsersService();

        // GET: OverValidLogin
        public ActionResult Index()
        {
            UsersFilterParameter filter = new UsersFilterParameter()
            {
            };
            
            IEnumerable<UserLoginViewModel> iquery = UsersService.GetUserLoginByFilter(filter);
            iquery = iquery.OrderByDescending(a => a.Id);
            List<UserLoginViewModel> result = iquery.ToList();

            return View(result);
        }
    }
}