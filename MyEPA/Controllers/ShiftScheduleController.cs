using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Services;
using MyEPA.Utility;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ShiftScheduleController : LoginBaseController
    {
        ShiftScheduleService ShiftScheduleService = new ShiftScheduleService();
        TeamShiftScheduleService TeamShiftScheduleService = new TeamShiftScheduleService();
        MainShiftScheduleService MainShiftScheduleService = new MainShiftScheduleService();
        DiasterService DiasterService = new DiasterService();
        DepartmentService DepartmentService = new DepartmentService();

        UsersService UsersService = new UsersService();
        public ActionResult Index(int departmentId, int diasterId = 0)
        {
            var result = ShiftScheduleService.GetByDepartmentId(departmentId, diasterId);

            ViewBag.DiasterId = diasterId;

            ViewBag.Diasters = DiasterService.GetAll();

            ViewBag.Users = UsersService
                .GetUsersJoinPositionByFilter(new UsersJoinPositionFilterParameter
                {
                    MaxRank = PositionRankEnum.Rank60.ToInteger(),
                    DepartmentIds = departmentId.ToListCollection()
                });

            ViewBag.DepartmentId = departmentId;

            ViewBag.Departments = DepartmentService.GetByIds(ShiftScheduleUtility.ShiftScheduleDepartmentIds);

            ViewBag.Managers = UsersService
                .GetUsersJoinPositionByFilter(new UsersJoinPositionFilterParameter
                {
                    MinRankContain = PositionRankEnum.Rank60.ToInteger(),
                    DepartmentIds = departmentId.ToListCollection()
                });

            return View(result);
        }
        public ActionResult Create(int departmentId, int diasterId = 0)
        {
            ShiftScheduleService.Create(departmentId, diasterId);
            return RedirectToAction("Index",new { diasterId, departmentId });
        }
        [HttpPost]
        public ActionResult Edit(int departmentId, List<ShiftScheduleViewModel> models)
        {
            if(models != null)
            {
                ShiftScheduleService.Update(departmentId, models);
            }
           
            return JsonResult(new
            {
                IsSuccess = true
            });
        }

        public ActionResult Delete(int departmentId, int diasterId, int id)
        {
            ShiftScheduleService.Delete(id);
            return RedirectToAction("Index", new { diasterId, departmentId });
        }

        public ActionResult Moving(int departmentId, int diasterId, int id,int changId)
        {
            ShiftScheduleService.Moving(id, changId);
            return RedirectToAction("Index", new { diasterId, departmentId });
        }

        public ActionResult GetUserShiftScheduleCount(DateTime? startTime, DateTime? endTime, int diasterId = 0, int? departmentId = null)
        {
            DateTime now = DateTimeHelper.GetCurrentTime();
            if (startTime.HasValue == false)
            {
                startTime = now.To_00_00_00();
            }
            if (endTime.HasValue == false)
            {
                endTime = startTime.Value.To_23_59_59();
            }
            ViewBag.Diasters = DiasterService.GetAll();
            ViewBag.DiasterId = diasterId;
            ViewBag.StartTime = startTime;
            ViewBag.EndTime = endTime;
            return View(ShiftScheduleService.GetUserShiftScheduleCount(new UserShiftScheduleCountFilterModel
            {
                DepartmentId = departmentId,
                DiasterId = diasterId,
                EndTime = endTime.Value,
                StartTime = startTime.Value
            }));
        }

        public ActionResult Check()
        {
            AdminResultModel result = ShiftScheduleService.Check(GetUserId());
            AdminResultModel teamResult = TeamShiftScheduleService.Check(GetUserId());
            AdminResultModel mainResult = MainShiftScheduleService.Check(GetUserId());
            
            if(result.IsSuccess || teamResult.IsSuccess || mainResult.IsSuccess)
            {
                return JsonResult(new AdminResultModel
                {
                    IsSuccess = true
                });
            }

            return JsonResult(new AdminResultModel
            {
                IsSuccess = false,
                ErrorMessage = "簽到失敗或不需要簽到"
            });
        }
        /// <summary>
        /// 簽到查詢
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchCheck(int diasterId = 0, int departmentId = 4)
        {
            ViewBag.DiasterId = diasterId;

            ViewBag.Diasters = DiasterService.GetAll();

            ViewBag.DepartmentId = departmentId;

            ViewBag.Departments = DepartmentService.GetAll();
            List<ShiftScheduleCheckViewModel> result = null;

            if (ShiftScheduleUtility.ShiftScheduleDepartmentIds.Contains(departmentId))
            {
                result = ShiftScheduleService.GetSearchCheck(departmentId, diasterId);
            }
            else if (ShiftScheduleUtility.TeamShiftScheduleDepartmentIds.Contains(departmentId))
            {
                result = new TeamShiftScheduleService().GetSearchCheck(departmentId, diasterId);
            }
            else
            {
                result = new MainShiftScheduleService().GetSearchCheck(departmentId, diasterId);
            }

            return View(result);
        }

        public ActionResult QueryAllShiftSchedule(int diasterId = 0,DateTime? date = null,bool? isNight = null)
        {
            if (date.HasValue == false)
            {
                date = DateTimeHelper.GetCurrentTime();
            }

            ViewBag.DiasterId = diasterId;

            ViewBag.Diasters = DiasterService.GetAll();

            ViewBag.IsNight = isNight;

            ViewBag.Date = date;

            return View(ShiftScheduleService.QueryAllShiftSchedule(diasterId, date.Value, isNight));
        }
    }
}