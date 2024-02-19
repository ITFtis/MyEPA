using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class Meeting_SignService
    {
        Meeting_SignRepository Meeting_SignRepository = new Meeting_SignRepository();
        Meeting_Sign_DetailRepository Meeting_Sign_DetailRepository = new Meeting_Sign_DetailRepository();
        public List<Meeting_SignViewModel> GetAll()
        {
            var dic = Meeting_Sign_DetailRepository.GetMeeting_SignAttendance();
            var result = Meeting_SignRepository.GetList().Select(e => new Meeting_SignViewModel
            {
                Attendance = dic.GetValue(e.Meeting_ID),
                MaximumAttendance = e.MaximumAttendance,
                del = e.del,
                Meeting_Address = e.Meeting_Address,
                Meeting_Datetime = e.Meeting_Datetime,
                Meeting_Food = e.Meeting_Food,
                Meeting_Food_Begin = e.Meeting_Food_Begin,
                Meeting_Food_End = e.Meeting_Food_End,
                Meeting_ID = e.Meeting_ID,
                Meeting_Issue = e.Meeting_Issue,
                Meeting_Keyin_date = e.Meeting_Keyin_date,
                Meeting_Keyin_name = e.Meeting_Keyin_name,
                Meeting_Memo = e.Meeting_Memo,
                Meeting_name = e.Meeting_name,
                Meeting_people_Sum = e.Meeting_people_Sum,
                Meeting_Sign_BeginTime = e.Meeting_Sign_BeginTime,
                Meeting_Sign_EndTime = e.Meeting_Sign_EndTime,
                Meeting_Traffic = e.Meeting_Traffic
            }).OrderBy(e => e.Meeting_Datetime).ToList();
            return result;
        }

        public void Create(UserBriefModel userBriefModel, Meeting_SignModel model)
        {
            model.Meeting_Keyin_name = userBriefModel.UserName;
            model.Meeting_Keyin_date = DateTimeHelper.GetCurrentTime();
            Meeting_SignRepository.Create(model);
        }

        public AdminResultModel Delete(int id)
        {
            var entity = Meeting_SignRepository.Get(id);

            if (entity == null)
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "資料不存在"
                };
            try
            {
                Meeting_SignRepository.Delete(id);
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

        public void Update(UserBriefModel userBriefModel, Meeting_SignModel model)
        {
            var entity = Meeting_SignRepository.Get(model.Meeting_ID);
            if(entity == null)
            {
                return;
            }
            entity.MaximumAttendance = model.MaximumAttendance;
            entity.Meeting_Address = model.Meeting_Address;
            entity.Meeting_Datetime = model.Meeting_Datetime;
            entity.Meeting_Food = model.Meeting_Food;
            entity.Meeting_Food_Begin = model.Meeting_Food_Begin;
            entity.Meeting_Food_End = model.Meeting_Food_End;
            entity.Meeting_Issue = model.Meeting_Issue;
            entity.Meeting_Memo = model.Meeting_Memo;
            entity.Meeting_name = model.Meeting_name;
            entity.Meeting_Sign_BeginTime = model.Meeting_Sign_BeginTime;
            entity.Meeting_Sign_EndTime = model.Meeting_Sign_EndTime;
            entity.Meeting_Traffic = model.Meeting_Traffic;
            entity.Meeting_Keyin_name = userBriefModel.UserName;
            entity.Meeting_Keyin_date = DateTimeHelper.GetCurrentTime();
            Meeting_SignRepository.Update(entity);
        }

        public Meeting_SignModel Get(int id)
        {
            var entity = Meeting_SignRepository.Get(id);
            return entity;
        }
    }
}