using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Services;

namespace MyEPA.Controllers
{
    public class EPBxDisinfectorController : LoginBaseController
    {       
        DisinfectorModel B= new DisinfectorModel();
        DisinfectorModel Item = new DisinfectorModel();
        LinkedList<DisinfectorModel> ItemList = new LinkedList<DisinfectorModel>();

        DisinfectorService DisinfectorService = new DisinfectorService();

        public ActionResult Add()
        {
            int SecondsCount = (int)(DateTime.UtcNow - new DateTime(2000, 1, 1)).TotalSeconds;
            MapModel A = new MapModel();
            string City = Session["AuthenticateCity"].ToString().Trim();
            string CityCode = A.FindCode(City);
            string Id = SecondsCount + CityCode;
            string Town = "環保局";
            string ContactUnit = Request["ContactUnit"];
            string DisinfectInstrument = Request["DisinfectInstrument"];
            string Standard = Request["Standard"];
            string Amount = Request["Amount"];
            string ROCyear = Request["ROCyear"];
            int? UseType = Request["UseType"].TryToInt();
            string ResponseMsg = B.Add(Id, City, Town, ContactUnit, DisinfectInstrument, Standard, Amount, ROCyear, UseType);

            return RedirectToAction("C3x1Disinfector", "Cleaner");
        }

        public ActionResult Confirm()
        {
            String City = Session["AuthenticateCity"].ToString();
            StatisticsModel Statistics = new StatisticsModel();
            String ResponseMsg = Statistics.StoreConfirmTime("Disinfector", City);
            ViewBag.Msg = ResponseMsg;
            ViewBag.City = City;
            return RedirectToAction("C3x1Disinfector", "Cleaner");
        }

        public ActionResult Delete()
        {
            String Id = Request["DeleteId"];
            String City = Session["AuthenticateCity"].ToString();
            DisinfectorModel Disinfector= new DisinfectorModel();
            String ResponseMsg = Disinfector.Delete(Id, City);
            @ViewBag.Msg = ResponseMsg;
            ViewBag.City = City;

            DisinfectorModel Item = new DisinfectorModel();
            LinkedList<DisinfectorModel> ItemList = new LinkedList<DisinfectorModel>();

            ItemList = Item.Show(City);
            ViewBag.Data = ItemList;
            return RedirectToAction("C3x1Disinfector", "Cleaner");
        }

        public JsonResult Edit(int Id)
        {
            var model = DisinfectorService.Get(Id);
            if(model == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            
            return Json(new 
                { 
                    model.City,
                    model.Town,
                    model.ContactUnit,
                    model.DisinfectInstrument,
                    model.Standard,
                    model.Amount,
                    model.ROCyear,
                    model.UseType,
                }, JsonRequestBehavior.AllowGet);
        }

   

        public ActionResult Update()
        {
            DisinfectorModel Disinfector = new DisinfectorModel();
            string Id = Request["EditId"];
            string City = Session["AuthenticateCity"].ToString().Trim();
            string ContactUnit = Request["EditContactUnit"];
            string Town = "環保局";
            string Instrument = Request["EditDisinfectInstrument"];
            string Standard = Request["EditStandard"];
            string Amount = Request["EditAmount"];
            string ROCyear = Request["EditROCyear"];
            int? UseType = Request["EditUseType"].TryToInt();
            string Msg = Disinfector.Update(Id, City, Town, ContactUnit, Instrument, Standard, Amount, ROCyear, UseType);
            ViewBag.Msg = Msg;
            ViewBag.City = City;
            return RedirectToAction("C3x1Disinfector", "Cleaner");

        }
    }
}