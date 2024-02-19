using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Services;

namespace MyEPA.Controllers
{
    public class CleanerxDumpController : LoginBaseController
    {

        DumpModel Item = new DumpModel();       
        LinkedList<DumpModel> ItemList = new LinkedList<DumpModel>();
        MapModel Place = new MapModel();
        DumpService DumpService = new DumpService();

        public JsonResult Edit(int id)
        {
            DumpModel A = DumpService.Get(id);

            string Town = A.Town;
            decimal Xpos = A.Xpos;
            decimal Ypos = A.Ypos;
            string ContactUnit = A.ContactUnit;
            string Address = A.Address;
            string Area = A.Area;
            string EmergencyContactPerson = A.EmergencyContactPerson;
            string EmergencyContactPersonTitle = A.EmergencyContactPersonTitle;
            string EmergencyMobilePhone = A.EmergencyMobilePhone;
            string EmergencyPhoneDay = A.EmergencyPhoneDay;
            string EmergencyPhoneNight = A.EmergencyPhoneNight;

            ViewBag.Data = DumpService.GetByFilter(new DumpFilterParameter
            {
                CityIds = GetUserCityId().ToListCollection()
            });
            string city = GetUserCity();
            ViewBag.City = city;
            ViewBag.Xpos = A.Xpos;
            ViewBag.Ypos = A.Ypos;
            return Json(new { city, Town, ContactUnit, Xpos, Ypos, Address, Area, EmergencyContactPerson, EmergencyContactPersonTitle, EmergencyMobilePhone, EmergencyPhoneDay, EmergencyPhoneNight }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            //新增堆置場要先給資料ID
            //本ID由2000年至今之秒數＋城市數字(來自MapModel)組成
            //之前由秒數+UserName組成，但Javascript抓不到非數字資料

            //ID命名要唯一，本命名假設同一秒，同縣市無人同建資料
            //更安全方式是採用秒數＋用戶編號
            //因為同一個人不會在同秒內連續新增資料

            int SecondsCount = (int)(DateTime.UtcNow - new DateTime(2000, 1, 1)).TotalSeconds;

            DumpService.Create(new DumpModel
            {
                Address = Request["Address"],
                Area = Request["Area"],
                City = Session["AuthenticateCity"].ToString().Trim(),
                ContactUnit = Request["ContactUnit"],
                ConfirmTime = null,
                EmergencyContactPerson = Request["EmergencyContactPerson"],
                EmergencyContactPersonTitle = Request["EmergencyContactPersonTitle"],
                EmergencyMobilePhone = Request["EmergencyMobilePhone"],
                EmergencyPhoneDay = Request["EmergencyPhoneDay"],
                EmergencyPhoneNight = Request["EmergencyPhoneNight"],
                Town = GetUserTown(),
                UpdateTime = DateTimeHelper.GetCurrentTime(),
                Xpos = Convert.ToDecimal(Request["Xpos"]),
                Ypos = Convert.ToDecimal(Request["Ypos"]),
            });
            return RedirectToAction("C3x1Dump", "Cleaner");
        }

        public ActionResult Delete()
        {
            String Id = Request["DeleteId"];
            String City = Session["AuthenticateCity"].ToString();
            new DumpService().Delete(Id);

            return RedirectToAction("C3x1Dump", "Cleaner");
        }


        public ActionResult Update()
        {

            var model = DumpService.Get(Convert.ToInt32(Request["EditId"]));
            model.Town = Request["EditTown"];
            model.ContactUnit = Request["EditContactUnit"];
            model.Xpos = Convert.ToDecimal(Request["EditXpos"]);
            model.Ypos = Convert.ToDecimal(Request["EditYpos"]);
            model.Address = Request["EditAddress"];
            model.Area = Request["EditDisinfectArea"];
            model.EmergencyContactPerson = Request["EditEmergencyContactPerson"];
            model.EmergencyContactPersonTitle = Request["EditEmergencyContactPersonTitle"];
            model.EmergencyMobilePhone = Request["EditEmergencyMobilePhone"];
            model.EmergencyPhoneDay = Request["EditEmergencyPhoneDay"];
            model.EmergencyPhoneNight = Request["EditEmergencyPhoneNight"];
            DumpService.Update(model);

            return RedirectToAction("C3x1Dump", "Cleaner");
        }

    }
}