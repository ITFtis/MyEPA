using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class EPBController : LoginBaseController
    {
        NewsService NewsService = new NewsService();
        NoticeService NoticeService = new NoticeService();

        public ActionResult B1a1()
        {
            string City = Session["AuthenticateCity"].ToString();
            ViewBag.City = City;
            string DistrictName = City + "環保局";
            DistrictModel district = new DistrictService().GetByDistrictName(DistrictName);
            ViewBag.Id = district.Id;
            ViewBag.DistrictName = district.DistrictName;
            ViewBag.CoverArea = district.CoverArea;
            ViewBag.Address = district.Address;
            ViewBag.Phone = district.Phone;
            ViewBag.Fax = district.Fax;
            ViewBag.Human= district.Human;
            ViewBag.OutHuman = district.OutHuman;
            ViewBag.ReadyHuman = district.ReadyHuman;
            ViewBag.CleanCapacity = district.CleanCapacity;
            return View();
        }

        public ActionResult B1a2()
        {
            UserModel User = new UserModel();
            string City = Session["AuthenticateCity"].ToString();
            ViewBag.Data = User.Show(City);
            ViewBag.City = City;
            return View("~/Views/EPB/B1a2.cshtml");
        }

        public ActionResult B1a3()
        {
            VehicleModel Vehicle = new VehicleModel();

            string City = Session["AuthenticateCity"].ToString();
            ViewBag.Data = Vehicle.Show(City);
            ViewBag.City = City;
            return View("~/Views/EPB/B1a3.cshtml");
        }

  

        public ActionResult B1a4()
        {
            DisinfectorModel Item = new DisinfectorModel();
            LinkedList<DisinfectorModel> ItemList = new LinkedList<DisinfectorModel>();

            string City = Session["AuthenticateCity"].ToString();
            ItemList = Item.Show(City);

            //EPBLayout需要ViewBag.City的值以顯現縣市名稱
            ViewBag.City = City;
            ViewBag.Data = ItemList;
            ViewBag.Msg = string.Empty;

            return View();
        }


        public ActionResult B1a5()
        {
            DisinfectantModel Item = new DisinfectantModel();
            LinkedList<DisinfectantModel> ItemList = new LinkedList<DisinfectantModel>();

            string City = Session["AuthenticateCity"].ToString();
            ItemList = Item.Show(City);

            //EPBLayout需要ViewBag.City的值以顯現縣市名稱
            ViewBag.City = City;
            ViewBag.Data = ItemList;
            ViewBag.Msg = string.Empty;
            return View();
        }


        public ActionResult B1a6()
        {
            String City = GetUserCity();

            MapModel Place = new MapModel();
            Place = Place.FindGPS(City);

            //EPBLayout需要ViewBag.City的值以顯現縣市名稱
            ViewBag.City = City;
            ViewBag.Data= new DumpService().GetByFilter(new DumpFilterParameter 
            {
                CityIds = GetUserCityId().ToListCollection()
            });
            ViewBag.Msg = string.Empty;          
            ViewBag.Xpos = Place.Xpos;
            ViewBag.Ypos = Place.Ypos;
            return View();
        }
        public ActionResult B1a8()
        {         
            string City = Session["AuthenticateCity"].ToString();           
            ViewBag.City = City;
            VolunteerModel Item= new VolunteerModel();
            LinkedList<VolunteerModel> ItemList = new LinkedList<VolunteerModel>();
            ViewBag.Data = Item.Show(City);
            return View();
        }

        public ActionResult B1a9()
        {
            PestModel Item = new PestModel();
            LinkedList<PestModel> ItemList = new LinkedList<PestModel>();
            string City = Session["AuthenticateCity"].ToString();
            ItemList = Item.Show(City);
            //EPBLayout需要ViewBag.City的值以顯現縣市名稱
            ViewBag.City = City;
            ViewBag.Data = ItemList;
            ViewBag.Msg = string.Empty;
            return View();
        }

       

        public ActionResult B1a10()
        {
            LandfillModel Item = new LandfillModel();
            LinkedList<LandfillModel> ItemList = new LinkedList<LandfillModel>();
            string City = Session["AuthenticateCity"].ToString();
            ItemList = Item.GetAll();

            //EPBLayout需要ViewBag.City的值以顯現縣市名稱
            ViewBag.City = City;
            ViewBag.Data = ItemList;
            ViewBag.Msg = string.Empty;
            return View();
        }

      

        public ActionResult B1a11()
        {
            IncineratorModel Item = new IncineratorModel();
            LinkedList<IncineratorModel> ItemList = new LinkedList<IncineratorModel>();
            string City = Session["AuthenticateCity"].ToString();
            ItemList = Item.Show("ALL");

            //EPBLayout需要ViewBag.City的值以顯現縣市名稱
            ViewBag.City = City;
            ViewBag.Data = ItemList;
            ViewBag.Msg = string.Empty;
            return View();
        }

        

        public ActionResult B3d11()
        {
            ViewBag.PostList = NoticeService.GetAll();
            ViewBag.PostType = "notice";

            //以下的ViewBag不能刪除，因為頁面上方要顯現某縣市環保局
            ViewBag.City = Session["AuthenticateCity"].ToString();
            return View();
        }

       

        public ActionResult B3d12()
        {
            ViewBag.PostList = NewsService.GetAll();
            ViewBag.PostType = "news";

            ViewBag.City = Session["AuthenticateCity"].ToString();
            return View();
        }

        public ActionResult B6x1(string title)
        {
            ViewBag.Title = title;
            ViewBag.Data = new FileUploadRepository().Show();
            return View();
        }

        public ActionResult EPB()
        {
           ViewBag.City = Session["AuthenticateCity"].ToString();
            return View();
        }

    }
}