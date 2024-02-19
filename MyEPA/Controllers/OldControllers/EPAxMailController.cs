using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Models;
namespace MyEPA.Controllers
{
    public class EPAxMailController : LoginBaseController
    {

        MailModel Mail = new MailModel();
        //電郵查詢

        public ActionResult A9x5()
        {
            ViewBag.Data = Mail.Show("ALL");
            return View("~/Views/EPA/A9x5.cshtml");
        }

        //public ActionResult Send()
        //{
        //    string SendResult = string.Empty; string AddResult = string.Empty;

          
        //    string MyId = "86158943";
        //    string MyPwd = "funepa725";
        //    string Topic = Request["Topic"].ToString().Trim();
        //    string Content = Request["Content"].ToString().Trim();
        //    string PhoneNumber = Request["PhoneNumber"].ToString().Trim();
        //    TextModel Text = new TextModel();

        //    //以下程式發送簡訊
        //    bool SendOK = Text.Send(MyId, MyPwd, Topic, Content, PhoneNumber);
        //    if (SendOK)
        //    { SendResult = "簡訊已成功送出！"; }
        //    else { SendResult = "抱歉，簡訊未送出！"; }

        //    //以下程式儲存簡訊到 Text 表格
        //    AddResult = Text.Add(Topic, PhoneNumber, Content, SendResult);
        //    ViewBag.Msg = SendResult + AddResult;
        //    UserModel User = new UserModel();
        //    ViewBag.Data = User.Show("ALL");
        //    return View("~/Views/EPA/A9x5.cshtml");
        //}
        //public ActionResult A9x3()
        //{
        //    UserModel User = new UserModel();
        //    ViewBag.Data = User.Show("ALL");
        //    return View("~/Views/EPA/A9x5.cshtml");
        //}

        public ActionResult Search()
        {
            //string SearchDuty = Request["SearchDuty"].ToString().Trim();
            //string SearchCity = Request["SearchCity"].ToString().Trim();
            //string SearchHumanType = Request["SearchHumanType"].ToString().Trim();
            //string SearchMainContacter = Request["SearchMainContacter"].ToString().Trim();
            //UserModel User = new UserModel();
            //ViewBag.Data = User.Search(SearchDuty, SearchCity, SearchHumanType, SearchMainContacter);

            //ViewBag.City = Session["AuthenticateCity"].ToString();
           
            ViewBag.Data = Mail.Show("ALL");
            return View("~/Views/EPA/A9x5.cshtml");
        }

        public ActionResult SearchByTime()
        {
            string A = Request["BeginTime"].ToString();
            string B = (Request["EndTime"]).ToString();
            DateTime BeginDay = Convert.ToDateTime(A);
            DateTime EndDay = Convert.ToDateTime(B).AddDays(1);

            ViewBag.Data = Mail.SearchByDays(BeginDay, EndDay);

            return View("~/Views/EPA/A9x5.cshtml");
        }
    }
}



