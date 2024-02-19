using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Repositories;
using MyEPA.Services;

namespace MyEPA.Controllers
{
    public class EPAxTextController : LoginBaseController
    {
        //簡訊發送
        public ActionResult A9x3()
        {
            UserModel User = new UserModel();
            ViewBag.Data = User.Show("ALL");
            return View("~/Views/EPA/A9x3.cshtml");
        }


        public ActionResult Send()
        {
            string topic = Request["Topic"].ToString().Trim();
            string content = Request["Content"].ToString().Trim();
            string phoneNumbers = Request["PhoneNumber"].ToString().Trim();
            string resultMsg = string.Empty;

            try
            {
                new SendMessageService().SendMessageByMobiles(phoneNumbers.Split(','), topic, content);
                resultMsg = "簡訊已成功送出！";
            }
            catch(Exception ex)
            {
                resultMsg = ex.Message;
            }

            ViewBag.Msg = resultMsg + "簡訊已存檔。";
            UserModel User = new UserModel();
            ViewBag.Data = User.Show("ALL");
            return View("~/Views/EPA/A9x3.cshtml");
        }
       

        public ActionResult Search()
        {
            string SearchDuty = Request["SearchDuty"].ToString().Trim();
            
            UserModel User = new UserModel();

            if (SearchDuty == "未填")
            {
                ViewBag.Msg = "請先選擇機關類別";
                ViewBag.Data = User.Show("ALL");
            }
            else
            {
                string SearchCity = Request["SearchCity"].ToString().Trim();
                string SearchHumanType = Request["SearchHumanType"].ToString().Trim();
                string SearchMainContacter = Request["SearchMainContacter"].ToString().Trim();
                string SearchTown = Request["SearchTown"].ToString().Trim();
                ViewBag.Data = User.Search(SearchDuty, SearchCity, SearchTown, SearchHumanType, SearchMainContacter);
            }

            ViewBag.City = Session["AuthenticateCity"].ToString();
            return View("~/Views/EPA/A9x3.cshtml");
        }

        public ActionResult SearchByTime()
        {
            string A = Request["BeginTime"].ToString();
            string B = (Request["EndTime"]).ToString();
            DateTime BeginDay = Convert.ToDateTime(A);
            DateTime EndDay = Convert.ToDateTime(B).AddDays(1);
            TextModel Text = new TextModel();
            ViewBag.Data = Text.SearchByDays(BeginDay, EndDay);

            return View("~/Views/EPA/A9x4.cshtml");
        }
    }
}



