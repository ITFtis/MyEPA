using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Services;
using MyEPA.Utility;
using MyEPA.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class TeamShiftScheduleController : LoginBaseController
    {
        TeamShiftScheduleService TeamShiftScheduleService = new TeamShiftScheduleService();
        DiasterService DiasterService = new DiasterService();
        DepartmentService DepartmentService = new DepartmentService();

        UsersService UsersService = new UsersService();
        public ActionResult Index(int departmentId, int diasterId = 0)
        {
            var result = TeamShiftScheduleService.GetByDepartmentId(departmentId, diasterId);

            ViewBag.DiasterId = diasterId;

            ViewBag.Diasters = DiasterService.GetAll();

            ViewBag.Users = UsersService
                .GetUsersJoinPositionByFilter(new UsersJoinPositionFilterParameter
                {
                    MaxRank = PositionRankEnum.Rank60.ToInteger(),
                    DepartmentIds = departmentId.ToListCollection()
                });

            ViewBag.DepartmentId = departmentId;

            ViewBag.Departments = DepartmentService.GetByIds(ShiftScheduleUtility.TeamShiftScheduleDepartmentIds);

            return View(result);
        }
        public ActionResult Create(int departmentId, int diasterId = 0)
        {
            TeamShiftScheduleService.Create(departmentId, diasterId);
            return RedirectToAction("Index",new { diasterId, departmentId });
        }
        [HttpPost]
        public ActionResult Edit(int departmentId, List<TeamShiftScheduleViewModel> models)
        {
            if(models != null)
            {
                TeamShiftScheduleService.Update(departmentId, models);
            }
           
            return JsonResult(new
            {
                IsSuccess = true
            });
        }

        public ActionResult Delete(int diasterId, int departmentId, int id)
        {
            TeamShiftScheduleService.Delete(id);
            return RedirectToAction("Index", new { diasterId, departmentId });
        }

        public ActionResult Moving(int diasterId,int departmentId, int id,int changId)
        {
            TeamShiftScheduleService.Moving(id, changId);
            return RedirectToAction("Index", new { diasterId, departmentId });
        }
    }
}