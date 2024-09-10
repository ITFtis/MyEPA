using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Models;
namespace MyEPA.Controllers
{

    //以下是用戶以環境部(EPA)身分登入，然後需要與焚化場
    //互動時，所需要的Controller

    public class EPAxIncineratorController : LoginBaseController
    {          
        //MapModel不是用來產生地圖，而是用以查詢城市代表的數字碼
        MapModel Place = new MapModel();

        public ActionResult A4x1Incinerator()
        {
            //以下是在環境部使用者點選進入焚化廠的項目後
            //系統將所有縣市焚化廠，呼叫IncineratorModel
            //存於IncinteratorModel的Linklist中，再放置ViewBag.Data中
            //以讓A4x1Incinterator.cshtml的頁面能顯示目前所有焚化廠的資料

            IncineratorModel Item = new IncineratorModel();
            LinkedList<IncineratorModel> ItemList = new LinkedList<IncineratorModel>();
            ItemList = Item.Show("ALL");            
            ViewBag.Data = ItemList;
            ViewBag.Msg = string.Empty;

            //以下兩行程式，是為了要在下個頁面，預設地圖座標輸入
            //因為若沒設，兩個欄位空白，在環境部用戶儲存資料後，
            //無X,Y值的資料會造成Javascript當掉。先預設地圖中心在臺北

            ViewBag.Xpos = "25.039673";
            ViewBag.Ypos = "121.507812";   
            return View("~/Views/EPA/A4x1Incinerator.cshtml");
        }

        //環境部用戶在顯現焚化廠的網頁(ShowIncinteratorForEdit.cshtml)中，針對某筆資料
        //勾選了瀏覽，於是系統透過IncinteratorModel的GetItem方法，將該筆資料存入A中
        //之後A的資料會被Javascript程式，放入瀏覽項目的聯絡單位、縣市、鄉鎮...等空格中

        public JsonResult Edit(int Id)
        {
            IncineratorModel A = new IncineratorModel();
            A = A.GetItem(Id);
          
            decimal Xpos = A.Xpos;
            decimal Ypos = A.Ypos;
            string ContactUnit = A.ContactUnit;
            string DesignCapacity = A.DesignCapacity;
            string Address = A.Address;
            string City = A.City;
            string Town = A.Town;
            DateTime UpdateTime = A.UpdateTime;
            string ContactPerson = A.ContactPerson;
            string ContactPersonTitle = A.ContactPersonTitle;
            string ContactPhone = A.ContactPhone;


            //資料被放入瀏覽項目後，視窗下方的條列式焚化廠資料
            //仍需要顯示，因此呼叫IncinterModel以產生ItemList並放入ViewBag.Data中等待顯示

            IncineratorModel Item = new IncineratorModel();
            LinkedList<IncineratorModel> ItemList = new LinkedList<IncineratorModel>();
            ItemList = Item.Show("ALL");
            ViewBag.Data = ItemList;

            //地圖要依照該縣市中心顯現
            //所以呼叫MapModel，找到縣市中心點

            MapModel Taiwan = new MapModel();
            Place = Place.FindGPS(City);
            ViewBag.Xpos = Place.Xpos;
            ViewBag.Ypos = Place.Ypos;

            //從ID所調出的該筆焚化廠資料，透過Json檔，傳回給Javascript程式使用

            return Json(new { City, Town, ContactUnit, DesignCapacity,Xpos, Ypos, Address,ContactPerson, ContactPersonTitle, ContactPhone, UpdateTime = UpdateTime.ToString("yyyy/MM/dd HH:mm:ss")}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            IncineratorModel Incinerator = new IncineratorModel();
            LinkedList<IncineratorModel> IncineratorList = new LinkedList<IncineratorModel>();
     
            //本ID由2000年至今之秒數＋城市數字(來自MapModel)組成
            //之前由秒數+UserName組成，但Javascript抓不到非數字資料
            //本命名假設同一秒，同縣市無人同建資料
            //更安全方式是採用秒數＋用戶編號，因為同一個人不會在同秒內連續新增資料，之後再調整

            int SecondsCount = (int)(DateTime.UtcNow - new DateTime(2000, 1, 1)).TotalSeconds;
            MapModel Taiwan = new MapModel();
            string UserCity = "臺北市";
            string CityCode = Taiwan.FindCode(UserCity);

            //用戶點資料確認儲存後並呼叫Add方法後，
            //以下程式抓取表單資料

            string ContactUnit = Request["ContactUnit"];
            string Xpos = Request["Xpos"];
            string Ypos = Request["Ypos"];
            string Address = Request["Address"];
            string City = Request["City"];
            string Town= Request["Town"];
            string DesignCapacity = Request["DesignCapacity"];
            string ContactPerson = Request["ContactPerson"];
            string ContactPersonTitle = Request["ContactPersonTitle"];
            string ContactPhone = Request["ContactPhone"];

            //再把抓取的表單資料，傳入Incinerator的Add方法中，
            //以增加該筆資料，成功或失敗的訊息則傳回給Msg，並存於ViewBag.Msg中
            //在A4x1Incinerator.cshtml的某位置顯示新增成功或失敗的訊息

            string ResponseMsg = Incinerator.Add(ContactUnit, DesignCapacity,Xpos, Ypos, City, Town, Address,ContactPerson,ContactPersonTitle,ContactPhone);
            ViewBag.Msg = ResponseMsg;

            ////新增成功後要回到原來的View，
            ////而原來的View預設為顯示現有的焚化廠
            ////所以呼叫ItemList，把所有焚化場叫出來
            IncineratorModel Item = new IncineratorModel();
            LinkedList<IncineratorModel> ItemList = new LinkedList<IncineratorModel>();
            ItemList = Item.Show("ALL");
            ViewBag.Data = ItemList;

            //地圖要依照該縣市中心顯現。剛剛新增資料的縣市，不代表下一筆也要新增該縣市，
            //所以把UserCity設為臺北市，並呼叫MapModel找出它的中心點
            //再進入A4x1Incinerator.cshtml時定位

            Place = Place.FindGPS(UserCity);
            ViewBag.Xpos = Place.Xpos;
            ViewBag.Ypos = Place.Ypos;
            return RedirectToAction("A4x1Incinerator");
        }

        public ActionResult Delete()
        {
            String Id = Request["DeleteId"];
            IncineratorModel Item= new IncineratorModel();

            LinkedList<IncineratorModel> ItemList = new LinkedList<IncineratorModel>();
            String ResponseMsg = Item.Delete(Id);
            @ViewBag.Msg = ResponseMsg;

            //刪除成功後，回到原頁面。該頁面預設為顯示現有焚化場
            //所以呼叫Item.Show("ALL")，把該城市的資料叫出來

            ItemList = Item.Show("ALL");
            ViewBag.Data = ItemList;

            //以下是讓地圖重新以臺北市為中心

            string City = "臺北市";
            ViewBag.City = City;
            Place = Place.FindGPS(City);
            ViewBag.Xpos = Place.Xpos;
            ViewBag.Ypos = Place.Ypos;
            return View("~/Views/EPA/A4x1Incinerator.cshtml");
        }
    }
}