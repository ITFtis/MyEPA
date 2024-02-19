using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;

namespace MyEPA.Controllers
{
    public class CleanerxDisinfectorController : LoginBaseController
    {       

        DisinfectorModel B= new DisinfectorModel();
        DisinfectorModel Item = new DisinfectorModel();
        LinkedList<DisinfectorModel> ItemList = new LinkedList<DisinfectorModel>();
        DisinfectorRepository DisinfectorRepository = new DisinfectorRepository();
        public ActionResult Add()
        {

            string City = Session["AuthenticateCity"].ToString().Trim();
            string Town = Session["AuthenticateTown"].ToString().Trim();
            
            DisinfectorRepository.Create(new DisinfectorModel
            {
                Amount = Request["Amount"],
                City = City,
                ContactUnit = Request["ContactUnit"],
                Town = Town,
                DisinfectInstrument = Request["DisinfectInstrument"],
                Standard = Request["Standard"],
                ROCyear = Request["ROCyear"],
                UseType = Request["UseType"].TryToInt()
            });
            //string ResponseMsg = B.Add(Id, City, Town, ContactUnit, DisinfectInstrument, Standard, Amount, ROCyear);

            ViewBag.Msg = "新增成功";
            ViewBag.City = City;
            ViewBag.Town = Town;
            DisinfectorModel Item = new DisinfectorModel();
            LinkedList<DisinfectorModel> ItemList = new LinkedList<DisinfectorModel>();

            ItemList = Item.Search(City,Town);
            ViewBag.Data = ItemList;
            return RedirectToAction("C3x1Disinfector", "Cleaner");
        }

    
        public ActionResult Delete()
        {
            String Id = Request["DeleteId"];
            String City = Session["AuthenticateCity"].ToString();
            String Town = Session["AuthenticateTown"].ToString();
            DisinfectorModel Disinfector= new DisinfectorModel();
            String ResponseMsg = Disinfector.Delete(Id, City);
            ViewBag.Msg = ResponseMsg;
            ViewBag.City = City;
            ViewBag.Town = Town;
            DisinfectorModel Item = new DisinfectorModel();
            LinkedList<DisinfectorModel> ItemList = new LinkedList<DisinfectorModel>();

            ItemList = Item.Search(City,Town);
            ViewBag.Data = ItemList;
            return RedirectToAction("C3x1Disinfector", "Cleaner");

        }
        public ActionResult Confirm(int? townId = null)
        {
            var user = GetUserBrief();

            DisinfectorFilterParameter filterParameter = new DisinfectorFilterParameter 
            {
                CityIds = user.CityId.ToListCollection(),
            };
            switch(user.Duty)
            {
                case DutyEnum.EPB:
                    {
                        if (townId.HasValue)
                        {
                            filterParameter.TownIds = townId.Value.ToListCollection();
                        }
                    }
                    break;
                default:
                    filterParameter.TownIds = user.TownId.ToListCollection();
                    break;
            }
            
            List<DisinfectorModel> disinfectors = DisinfectorRepository.GetByFilter(filterParameter);

            foreach (var item in disinfectors)
            {
                item.ConfirmTime = DateTimeHelper.GetCurrentTime();
            }

            DisinfectorRepository.Update(disinfectors);

            return RedirectToAction("C3x1Disinfector", "Cleaner",new { townId });
        }
        public JsonResult Edit(string Id)
        {          
            string City = Session["AuthenticateCity"].ToString().Trim();
            Item = Item.GetItem(Id);
            string Town = Item.Town;
            string ContactUnit = Item.ContactUnit;
            string DisinfectInstrument = Item.DisinfectInstrument;
            string Standard = Item.Standard;
            string Amount = Item.Amount;
            string ROCyear = Item.ROCyear;

            ViewBag.City = City;
            ItemList = Item.Show(City);
            ViewBag.Data = ItemList;
            ViewBag.Msg = string.Empty;
            return Json(new { City, Town, ContactUnit, DisinfectInstrument,Standard,Amount,ROCyear}, JsonRequestBehavior.AllowGet);
        }

   

        public ActionResult Update()
        {
            DisinfectorModel Disinfector = new DisinfectorModel();
            string Id = Request["EditId"];
            string City = Session["AuthenticateCity"].ToString().Trim();
            string Town = Session["AuthenticateTown"].ToString().Trim();
            string ContactUnit = Request["EditContactUnit"];
            string Instrument = Request["EditDisinfectInstrument"];
            string Standard = Request["EditStandard"];
            string Amount = Request["EditAmount"];
            string ROCyear = Request["EditROCyear"];
            int? UseType = Request["EditUseType"].TryToInt();
            string Msg = Disinfector.Update(Id, City, Town, ContactUnit, Instrument, Standard, Amount, ROCyear, UseType);
            ViewBag.Msg = Msg;
            ViewBag.City = City;
            ViewBag.Town = Town;

            return RedirectToAction("C3x1Disinfector", "Cleaner");

        }
    }
}