using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Models;
namespace MyEPA.Controllers
{
    public class CleanerxResourceController : LoginBaseController
    {
    
        public ActionResult C1a()
        {
            ViewBag.City = Session["AuthenticateCity"].ToString();
            ViewBag.Town = Session["AuthenticateTown"].ToString();
            return View("~/Views/Cleaner/C1a.cshtml");
        }

        public ActionResult Show()
        {
            string SearchCity = Request["SearchCity"].ToString().Trim();
            string SearchType = Request["SearchType"].ToString().Trim();
            ViewBag.City = Session["AuthenticateCity"].ToString().Trim();
            ViewBag.Town = Session["AuthenticateTown"].ToString();
            if (SearchType == "Disinfector")
            {
                DisinfectorModel Disinfector = new DisinfectorModel();
                ViewBag.Data = Disinfector.Show(SearchCity);
                return View("~/Views/Cleaner/C1aDisinfector.cshtml");
            }
            else if (SearchType == "Vehicle")
            {
                VehicleModel Vehicle = new VehicleModel();
                ViewBag.Data = Vehicle.Show(SearchCity);
                return View("~/Views/Cleaner/C1aVehicle.cshtml");
            }
            else if (SearchType == "Disinfectant")
            {
                DisinfectantModel Disinfectant = new DisinfectantModel();
                ViewBag.Data = Disinfectant.Show(SearchCity);
                return View("~/Views/Cleaner/C1aDisinfectant.cshtml");
            }
            else if (SearchType == "Landfill")
            {
                LandfillModel Landfill = new LandfillModel();
                ViewBag.Data = Landfill.Show(SearchCity);
                return View("~/Views/Cleaner/C1aLandfill.cshtml");
            }
            else if (SearchType == "Incinerator")
            {
                IncineratorModel Incinerator = new IncineratorModel();
                ViewBag.Data = Incinerator.Show(SearchCity);
                return View("~/Views/Cleaner/C1aIncinerator.cshtml");
            }
            else
            {
                ViewBag.Data = null;
                return View("~/Views/Cleaner/C1a.cshtml");
            }
          
        }

        
    }
}



