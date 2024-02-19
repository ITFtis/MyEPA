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
    public class VolunteerController : LoginBaseController
    {
        VolunteerService VolunteerService = new VolunteerService();

        public ActionResult Search(VolunteerFilterParameter filter)
        {
            var result = VolunteerService.GetByFilter(filter);

            return View(result);
        }
    }
}