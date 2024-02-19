using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class Meeting_Sign_DetailController : LoginBaseController
    {
        Meeting_SignService Meeting_SignService = new Meeting_SignService();
        Meeting_Sign_DetailService Meeting_Sign_DetailService = new Meeting_Sign_DetailService();

        public ActionResult Index(int meetingid, string message = null)
        {
            var result = Meeting_Sign_DetailService.GetListByMeetingId(meetingid);

            var meeting = Meeting_SignService.Get(meetingid);
            ViewBag.Attendance = Meeting_Sign_DetailService.GetMeeting_SignAttendanceByMeetingId(meetingid);
            ViewBag.Meeting = meeting;
            ViewBag.Message = message;
            ViewBag.Meetingid = meetingid;

            return View(result);
        }

        public ActionResult Create(int meetingid)
        {
            return View(new Meeting_Sign_DetailModel 
            { 
                Meeting_ID = meetingid
            });
        }

        [HttpPost]
        public ActionResult Create(Meeting_Sign_DetailModel model)
        {
            var result = Meeting_Sign_DetailService.CreateAndResultIdentity(model);
            return RedirectToIndex(model.Meeting_ID, result.ErrorMessage);
        }

        public ActionResult Edit(int id)
        {
            var result = Meeting_Sign_DetailService.Get(id);

            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(Meeting_Sign_DetailModel model)
        {
            Meeting_Sign_DetailService.Update(GetUserBrief(), model);
            return RedirectToIndex(model.Meeting_ID);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = Meeting_Sign_DetailService.Delete(id);
            return JsonResult(result);
        }
        private RedirectToRouteResult RedirectToIndex(int meetingid,string message = null)
        {
            return RedirectToAction("Index", new { meetingid, message });
        }
    }
}