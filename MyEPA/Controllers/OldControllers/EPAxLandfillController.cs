using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Extensions;
using MyEPA.Models;
namespace MyEPA.Controllers
{
    public class EPAxLandfillController : LoginBaseController
    {
        StatisticsModel Statistics = new StatisticsModel();
        LandfillModel Landfill = new LandfillModel();
        DataContext DataStorage = new DataContext();
        LinkedList<LandfillModel> LandfillList = new LinkedList<LandfillModel>();

        public ActionResult A4x1Landfill()
        {
            LandfillList = Landfill.GetAll();
            ViewBag.Data = LandfillList;           
            ViewBag.Msg = string.Empty;
            ViewBag.Xpos = "25.0375417";
            ViewBag.Ypos = "121.5622494";
            return View("~/Views/EPA/A4x1Landfill.cshtml");
        }

        public ActionResult Add()
        {
            //ID由2000年至今之秒數＋城市數組成，之前由秒數+UserName組成，
            //但Javascript抓不到非數字資料，本命名假設同秒同縣市無人同建資料，
            //更安全方式是採秒數＋用戶數字編號，因同人不會在同秒內新增資料

            int SecondsCount = (int)(DateTime.UtcNow - new DateTime(2000, 1, 1)).TotalSeconds;
            MapModel Taiwan = new MapModel();
            string UserCity = "臺北市";
            string CityCode = Taiwan.FindCode(UserCity);
            string Id = SecondsCount + CityCode;

            String ContactUnit = Request["ContactUnit"];
            string Xpos = Request["Xpos"];
            string Ypos = Request["Ypos"];
            string City = Request["City"];
            string Town = Request["Town"];
            string ContactPerson = Request["ContactPerson"];       
            string ContactPersonTitle = Request["ContactPersonTitle"];
            string ContactPhone = Request["ContactPhone"];
            string DesignCapacity = Request["DesignCapacity"];
            string ResidualCapacity = Request["ResidualCapacity"];
            string Address = Request["Address"];
            string IsDump = Request["IsDump"];
            string ResponseMsg = Landfill.Add(Id, ContactUnit, Xpos, Ypos,City,Town, ContactPerson,ContactPersonTitle,ContactPhone, DesignCapacity, ResidualCapacity, Address,IsDump);
            @ViewBag.Msg = ResponseMsg;
            LandfillList = Landfill.GetAll();
            ViewBag.Data = LandfillList;
            return View("~/Views/EPA/A4x1LandFill.cshtml");
        }

        public ActionResult Delete()
        {
            String Id = Request["DeleteId"];
            LandfillModel Landfill = new LandfillModel();
            String ResponseMsg = Landfill.Delete(Id);
            @ViewBag.Msg = ResponseMsg;

            //因為刪除資料成功後，要回View頁面
            LandfillModel Item = new LandfillModel();
            LinkedList<LandfillModel> ItemList = new LinkedList<LandfillModel>();

            ItemList = Item.GetAll();
            ViewBag.Data = ItemList;

            return View("/Views/EPA/A4x1LandFill.cshtml");
        }

        public JsonResult Edit(string Id)
        {
            LandfillModel A = new LandfillModel();
            A = A.GetItem(Id);

            decimal Xpos = A.Xpos;
            decimal Ypos = A.Ypos;
            string ContactUnit = A.ContactUnit;
            string DesignCapacity = A.DesignCapacity;
            string ResidualCapacity = A.ResidualCapacity;
            string Address = A.Address;
            string City = A.City;
            string Town = A.Town;
            DateTime UpdateTime = A.UpdateTime;
            string ContactPerson = A.ContactPerson;
            string ContactPersonTitle = A.ContactPersonTitle;
            string ContactPhone = A.ContactPhone;
            string IsDump = A.IsDump;
            IncineratorModel Item = new IncineratorModel();
            LinkedList<IncineratorModel> ItemList = new LinkedList<IncineratorModel>();
            ItemList = Item.Show("ALL");
            ViewBag.Data = ItemList;


            //地圖要依照該縣市中心顯現
            //所以呼叫MapModel，找到縣市中心點
            //用戶是環境部，所以City設為臺北市

            MapModel Taiwan = new MapModel();
            Taiwan = Taiwan.FindGPS(City);
            ViewBag.Xpos = Taiwan.Xpos;
            ViewBag.Ypos = Taiwan.Ypos;

            return Json(new 
            { 
                City, 
                Town, 
                ContactUnit,
                DesignCapacity, 
                ResidualCapacity,
                Xpos, 
                Ypos, 
                Address, 
                ContactPerson,
                ContactPersonTitle, 
                ContactPhone,IsDump,
                UpdateTime = UpdateTime.ToDateTimeString()
            }, JsonRequestBehavior.AllowGet);
        }

        //以下是舊方式來刪除垃圾掩埋場，亦即利用EntityFramework產生等待刪除資料的GridView表格，
        //放在PickLandfillToRemove.cshtml上。目前其它物件是用HTML Table直接建立表格，
        //等垃圾延埋場也採用新方法後刪除

        public ActionResult PickLandfillToRemove()
        {
            var data = DataStorage.DeleteLandfill;
            return View("~/Views/EPA/PickLandfillToRemove.cshtml",data.ToList());
        }

        //以下是舊方式來刪除掩埋場，亦即在GridView上擺按鈕，
        //Delete參數會抓取該行的Id，再利用Landfill.Delete
        //去刪除該筆資料

        [HttpPost]
        public ActionResult DoRemove(string Delete)
        {
            string ResponeMsg = Landfill.Delete(Delete);
            ViewBag.Msg = ResponeMsg;
            LandfillList = Landfill.GetAll();
            ViewBag.Data = LandfillList;
            return View("/Views/EPA/A4x1LandFill.cshtml");
        }
    }
}