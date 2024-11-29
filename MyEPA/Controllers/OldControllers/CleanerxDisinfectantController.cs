using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;

namespace MyEPA.Controllers
{
    public class CleanerxDisinfectantController : LoginBaseController
    {
        DisinfectantModel B = new DisinfectantModel();

        DisinfectantModel Item = new DisinfectantModel();
        LinkedList<DisinfectantModel> ItemList = new LinkedList<DisinfectantModel>();

        DisinfectantRepository DisinfectantRepository = new DisinfectantRepository();

        public ActionResult Add(DisinfectantModel model)
        {
            var user = GetUserBrief();

            bool isSupportCity = Request["IsSupportCity"].ToString() == "" ? false : bool.Parse(Request["IsSupportCity"]);
            int? supportCityNum = null;

            if (isSupportCity)
            {
                //有跨縣市才會有數量
                if (Request["SupportCityNum"].ToString() != "")
                    supportCityNum = int.Parse(Request["SupportCityNum"]);
            }

            model.UpdateUser = user.UserName;
            model.UpdateTime = DateTimeHelper.GetCurrentTime();
            model.ConfirmTime = model.UpdateTime;
            model.City = user.City;
            model.Town = user.Town;
            model.IsSupportCity = isSupportCity;
            model.SupportCityNum = supportCityNum;
            DisinfectantRepository.Create(model);
            ViewBag.Msg = "新增成功";

            return RedirectToAction("C3x1Disinfectant", "Cleaner");
        }


        public ActionResult Confirm(int? townId = null)
        {
            var user = GetUserBrief();
            DisinfectantFilterParameter filterParameter = new DisinfectantFilterParameter
            {
                CityIds = user.CityId.ToListCollection(),
            };
            switch(_Duty)
            {
                case DutyEnum.EPB:
                    if (townId.HasValue)
                    {
                        filterParameter.TownIds = townId.Value.ToListCollection();
                    }
                    break;
                default:
                    filterParameter.TownIds = user.TownId.ToListCollection();
                    break;
            }
            List<DisinfectantModel> disinfectants = DisinfectantRepository.GetByFilter(filterParameter);

            foreach (var item in disinfectants)
            {
                item.UpdateUser = user.UserName;
                item.ConfirmTime = DateTimeHelper.GetCurrentTime();
            }

            DisinfectantRepository.Update(disinfectants);

            return RedirectToAction("C3x1Disinfectant", "Cleaner",new { townId });
        }


        public ActionResult Delete()
        {
            DisinfectantRepository.Delete(Request["DeleteId"].TryToInt().GetValueOrDefault());

            string City = Session["AuthenticateCity"].ToString();
            string Town = Session["AuthenticateTown"].ToString();
            
            LinkedList<DisinfectantModel> ItemList = new DisinfectantModel().Search(City, Town);

            ViewBag.Msg = "已刪除資料";

            return RedirectToAction("C3x1Disinfectant", "Cleaner");
        }

        public JsonResult Edit(int Id)
        {
            string City = Session["AuthenticateCity"].ToString().Trim();
            Item = new DisinfectantRepository().Get(Id);
            string Town = Item.Town;
            string ContactUnit = Item.ContactUnit;
            string DrugName = Item.DrugName;
            string DrugType = Item.DrugType;
            string DrugState = Item.DrugState;


            string ActiveIngredients1 = Item.ActiveIngredients1;
            string ActiveIngredients2 = Item.ActiveIngredients2;


            decimal Amount = Item.Amount;
            string Density = Item.Density;
            decimal Area = Item.Area;
            int UseType = Item.UseType;
            bool? IsSupportCity = Item.IsSupportCity;
            int? SupportCityNum = Item.SupportCityNum;
            //DateTime ServiceLife = Item.ServiceLife;            
            string ServiceLife = Item.ServiceLife == null ? "" : ((DateTime)Item.ServiceLife).ToString("yyyy-MM-dd");
            //因為編輯資料後，要回B1a4的View頁面，ˋ
            //而B1a4預設為顯示現有臨時垃圾堆置場
            //所以呼叫Dump，把該城市的資料叫出來
            ViewBag.City = City;
            ItemList = Item.Show(City);
            ViewBag.Data = ItemList;
            ViewBag.Msg = string.Empty;

            return Json(new { ActiveIngredients1, ActiveIngredients2, City, Town, ContactUnit, DrugName, DrugType, DrugState, Amount, Density, Area, ServiceLife = ServiceLife, UseType, IsSupportCity, SupportCityNum }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Update()
        {
            var user = GetUserBrief();
            string UserName = user.UserName;

            DisinfectantModel Disinfectant = new DisinfectantModel();
            string Id = Request["EditId"];
            string City = Session["AuthenticateCity"].ToString().Trim();
            string Town = Session["AuthenticateTown"].ToString().Trim();
            string ContactUnit = Request["EditContactUnit"];
            string DrugName = Request["EditDrugName"]== "其他"? Request["EditDrugNameText"] : Request["EditDrugName"];
            string DrugType = Request["EditDrugType"];
            string DrugState = Request["EditDrugState"];
            string Amount = Request["EditAmount"];
            string Density = Request["EditDensity"];
            string Area = Request["EditArea"];
            string ServiceLife = Request["EditServiceLife"];
            string ActiveIngredients1 = Request["ActiveIngredients1"];
            string ActiveIngredients2 = Request["ActiveIngredients2"];
            int UseType = Request["EditUseType"].TryToInt().GetValueOrDefault();
            bool IsSupportCity = Request["IsSupportCity"].ToString() == "" ? false : bool.Parse(Request["IsSupportCity"]);
            int? SupportCityNum = null;

            if (IsSupportCity)
            {
                //有跨縣市才會有數量
                if (Request["SupportCityNum"].ToString() != "")
                    SupportCityNum = int.Parse(Request["SupportCityNum"]);
            }

            string Msg = Disinfectant.Update(Id, City, Town, ContactUnit, DrugName, DrugType, DrugState, Amount, Density, Area, ServiceLife, UseType, ActiveIngredients1, ActiveIngredients2, UserName, IsSupportCity, SupportCityNum);

            return RedirectToAction("C3x1Disinfectant", "Cleaner");
        }
    }
}