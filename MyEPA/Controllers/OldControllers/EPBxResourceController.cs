using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Models;
namespace MyEPA.Controllers
{
    public class EPBxResourceController : LoginBaseController
    {

        public ActionResult B3a()
        {
            ViewBag.City = Session["AuthenticateCity"].ToString();
            return View("~/Views/EPB/B3a.cshtml");
        }

        public ActionResult Show()
        {
            string SearchCity = Request["SearchCity"].ToString().Trim();
            string SearchType = Request["SearchType"].ToString().Trim();
            ViewBag.City = Session["AuthenticateCity"].ToString().Trim();
            if (SearchType == "Disinfector")
            {
                DisinfectorModel Disinfector = new DisinfectorModel();
                ViewBag.Data = Disinfector.Show(SearchCity);
                return View("~/Views/EPB/B3aDisinfector.cshtml");
            }
            else if (SearchType == "Vehicle")
            {
                VehicleModel Vehicle= new VehicleModel();
                ViewBag.Data = Vehicle.Show(SearchCity);
                return View("~/Views/EPB/B3aVehicle.cshtml");
            }
            else if (SearchType == "Disinfectant")
            {
                DisinfectantModel Disinfectant = new DisinfectantModel();
                ViewBag.Data = Disinfectant.Show(SearchCity);
                return View("~/Views/EPB/B3aDisinfectant.cshtml");
            }
            else if (SearchType == "Landfill")
            {
                LandfillModel Landfill = new LandfillModel();
                ViewBag.Data = Landfill.Show(SearchCity);
                return View("~/Views/EPB/B3aLandfill.cshtml");
            }
            else if (SearchType == "Incinerator")
            {
                IncineratorModel Incinerator = new IncineratorModel();
                ViewBag.Data = Incinerator.Show(SearchCity);
                return View("~/Views/EPB/B3aIncinerator.cshtml");
            }
           
            else
            {
                ViewBag.Data = null;
                return View("~/Views/EPB/B3a.cshtml");
            }
          
        }

        
    }
}



