using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Models;
namespace MyEPA.Controllers
{
    public class EPBxVolunteerController : LoginBaseController
    {
        public ActionResult Delete()
        {
            String Id = Request["DeleteId"];
            String City = Session["AuthenticateCity"].ToString();
            VolunteerModel Item = new VolunteerModel();
            String ResponseMsg = Item.Delete(Id, City);
            @ViewBag.Msg = ResponseMsg;
            ViewBag.City = City;
            
            LinkedList<VolunteerModel> ItemList = new LinkedList<VolunteerModel>();

            ItemList = Item.Show(City);
            ViewBag.Data = ItemList;

            return View("/Views/EPB/B1a8.cshtml");
        }

        public ActionResult Confirm()
        {
            String City = Session["AuthenticateCity"].ToString();
            StatisticsModel Statistics = new StatisticsModel();
            String ResponseMsg = Statistics.StoreConfirmTime("Volunteer", City);
            ViewBag.Msg = ResponseMsg;
            ViewBag.City = City;
            VolunteerModel Item = new VolunteerModel();
            LinkedList<VolunteerModel> ItemList = new LinkedList<VolunteerModel>();
            ViewBag.Data = Item.Show(City);
            return View("/Views/EPB/B1a8.cshtml");
        }

        public ActionResult Add()
        {
            int SecondsCount = (int)(DateTime.UtcNow - new DateTime(2000, 1, 1)).TotalSeconds;
            MapModel A = new MapModel();
            string City = Session["AuthenticateCity"].ToString().Trim();
            string CityCode = A.FindCode(City);
            string Id = SecondsCount + CityCode;
            string ContactPerson = Request["ContactPerson"];
            string Phone = Request["Phone"];
            string MobilePhone = Request["MobilePhone"];
            string Fax = Request["Fax"];
            string Mail =  Server.HtmlEncode(Request["Mail"]);
            string Service = Request["Service"];
            VolunteerModel Item = new VolunteerModel();
            string ResponseMsg = Item.Add(Id, City, ContactPerson, Phone, MobilePhone, Fax,Mail,Service);
            ViewBag.Msg = ResponseMsg;
            ViewBag.City = City;

            LinkedList<VolunteerModel> ItemList = new LinkedList<VolunteerModel>();
            ItemList = Item.Show(City);
            ViewBag.Data = ItemList;

            return View("~/Views/EPB/B1a8.cshtml");
        }
    }
}