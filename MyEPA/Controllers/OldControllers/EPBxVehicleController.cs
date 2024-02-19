using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Models;
namespace MyEPA.Controllers
{
    public class EPBxVehicleController : LoginBaseController
    {         
        public ActionResult Search()
        {      
            string City = Session["AuthenticateCity"].ToString().Trim();
            string Town = Request["SearchTown"];
            VehicleModel Item = new VehicleModel();
            LinkedList<VehicleModel> ItemList = new LinkedList<VehicleModel>();
            ItemList = Item.Show(City,Town);
            ViewBag.Data = ItemList;
            ViewBag.City = City;
            return View("~/Views/EPB/B1a3.cshtml");
        }
        public ActionResult Reset()
        {
            return RedirectToAction("B1a3", "EPB");
        }
    }
}