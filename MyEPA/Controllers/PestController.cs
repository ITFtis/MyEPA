using MyEPA.Extensions;
using MyEPA.Models.FilterParameter;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class PestController : LoginBaseController
    {
        PestService PestService = new PestService();

        public ActionResult Search(PestFilterParameter filter)
        {
            var result = PestService.GetByFilter(filter);

            return View(result);
        }
    }
}