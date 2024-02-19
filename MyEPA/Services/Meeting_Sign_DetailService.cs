using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;

namespace MyEPA.Services
{
    public class Meeting_Sign_DetailService
    {
        Meeting_SignRepository Meeting_SignRepository = new Meeting_SignRepository();
        Meeting_Sign_DetailRepository Meeting_Sign_DetailRepository = new Meeting_Sign_DetailRepository();
        public List<Meeting_Sign_DetailModel> GetAll()
        {
            return Meeting_Sign_DetailRepository.GetList();
        }
        public Dictionary<int, int> GetMeeting_SignAttendance()
        {
            return Meeting_Sign_DetailRepository.GetMeeting_SignAttendance();
        }
        public int GetMeeting_SignAttendanceByMeetingId(int meetingId)
        {
            return GetMeeting_SignAttendance().GetValue(meetingId);
        }
        public AdminResultModel CreateAndResultIdentity(Meeting_Sign_DetailModel model)
        {
            var meeting = Meeting_SignRepository.Get(model.Meeting_ID);
           
            if(meeting == null)
            {
                return new AdminResultModel 
                {
                    IsSuccess = false,
                    ErrorMessage = "報名失敗！該報名不存在"
                };
            }

            DateTime now = DateTimeHelper.GetCurrentTime();
            //尚未開始
            if (meeting.Meeting_Sign_BeginTime.HasValue && now < meeting.Meeting_Sign_BeginTime.Value)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "報名失敗！該報名尚未開始"
                };
            }
            //已經結束
            if (meeting.Meeting_Sign_EndTime.HasValue && now > meeting.Meeting_Sign_EndTime.Value)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "報名失敗！該報名已經截止"
                };
            }

            var attendance = GetMeeting_SignAttendanceByMeetingId(model.Meeting_ID);

            if (attendance >= meeting.MaximumAttendance)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "報名失敗！已達報名上限"
                };
            }

            model.Keyin_date = DateTimeHelper.GetCurrentTime();
            model.Source_IP = GetIPAddress();
            var rowId = Meeting_Sign_DetailRepository.CreateAndResultIdentity<int>(model);
            return new AdminResultModel
            {
                IsSuccess = true,
                ErrorMessage = $"報名成功！您的報名序號為 {rowId}"
            };
        }

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public List<Meeting_Sign_DetailModel> GetListByMeetingId(int meetingid)
        {
            return Meeting_Sign_DetailRepository.GetListByMeetingId(meetingid);
        }

        public Meeting_Sign_DetailModel Get(int id)
        {
            return Meeting_Sign_DetailRepository.Get(id);
        }

        public void Update(UserBriefModel user, Meeting_Sign_DetailModel model)
        {
            throw new NotImplementedException();
        }

        public AdminResultModel Delete(int id)
        {
            var entity = Meeting_Sign_DetailRepository.Get(id);

            if (entity == null)
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "資料不存在"
                };
            try
            {
                Meeting_Sign_DetailRepository.Delete(id);
            }
            catch (Exception ex)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
            return new AdminResultModel
            {
                IsSuccess = true
            };
        }
    }
}