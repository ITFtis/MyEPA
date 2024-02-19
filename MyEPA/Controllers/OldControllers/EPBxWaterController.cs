using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Models;
using MyEPA.Repositories;

namespace MyEPA.Controllers
{
    public class EPBxWaterController : LoginBaseController
    {
        DiasterRepository DiasterRepository = new DiasterRepository();
        DiasterBLModel Diaster = new DiasterBLModel();
        public ActionResult B1d()
        {
            ViewBag.City = Session["AuthenticateCity"].ToString();
            ViewBag.Data = DiasterRepository.GetList();
            return View("~/Views/EPB/B1d.cshtml");
        }
        public ActionResult B1dDiaster()
        {
            ViewBag.City = Session["AuthenticateCity"].ToString();
            string DiasterId = Request["DiasterId"];
            ViewBag.DiasterId = DiasterId;

            if (string.IsNullOrEmpty(DiasterId))
            {
                ViewBag.Msg = "請先選災害主題";
                ViewBag.Data = DiasterRepository.GetList();
                return View("~/Views/EPB/B1d.cshtml");
            }
            else
            {
                LinkedList<string> ActiveDays = new LinkedList<string>();
                ViewBag.DiasterName = Request["DiasterName"];
                string StartTime= Request["StartTime"];
                ViewBag.StartTime = StartTime;
                string EndTime = Request["EndTime"];
                ViewBag.EndTime = EndTime;
                DateTime Day1 = Convert.ToDateTime(StartTime);
                ActiveDays.AddLast(StartTime);
                string Temp = StartTime;
                int i = 1;
                while (Temp != EndTime)
                {
                    Temp = Day1.AddDays(i).ToString("yyyy-MM-dd");
                    ActiveDays.AddLast(Temp);
                    i = i + 1;
                }
                ViewBag.ActiveDays = ActiveDays;

                return View("~/Views/EPB/B1dDiaster.cshtml");
            }         
        }

        public ActionResult B1dWater()
        {
            ViewBag.City = Session["AuthenticateCity"].ToString();
            string DiasterId = Request["DiasterId"];
            string DiasterName = Request["DiasterName"];
            string ReportDay = Request["ReportDay"];
            if (string.IsNullOrEmpty(ReportDay))
            {
                ViewBag.Msg = "請先選日期";
                LinkedList<string> ActiveDays = new LinkedList<string>();
                ViewBag.DiasterName = Request["DiasterName"];
                ViewBag.DiasterId = Request["DiasterId"];
                string StartTime = Request["StartTime"];
                ViewBag.StartTime = StartTime;
                string EndTime = Request["EndTime"];
                ViewBag.EndTime = EndTime;
                DateTime Day1 = Convert.ToDateTime(StartTime);
                ActiveDays.AddLast(StartTime);
                string Temp = StartTime;
                int i = 1;
                while (Temp != EndTime)
                {
                    Temp = Day1.AddDays(i).ToString("yyyy-MM-dd");
                    ActiveDays.AddLast(Temp);
                    i = i + 1;
                }
                ViewBag.ActiveDays = ActiveDays;

                return View("~/Views/EPB/B1dDiaster.cshtml");
            }
            else
            {
                ViewBag.DiasterId = DiasterId;
                ViewBag.DiasterName = DiasterName;
                ViewBag.ReportDay = ReportDay;
                return View("~/Views/EPB/B1dWater.cshtml");
            }
        }

        public ActionResult Report()
        {
            ViewBag.City = Session["AuthenticateCity"].ToString();
            ViewBag.Data = DiasterRepository.GetList();
            return View("~/Views/EPB/B1d.cshtml");
        }
    }
}