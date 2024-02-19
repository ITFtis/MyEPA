using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class TeamShiftScheduleService
    {
        TeamShiftScheduleRepository TeamShiftScheduleRepository = new TeamShiftScheduleRepository();
        TeamShiftScheduleUserRepository TeamShiftScheduleUserRepository = new TeamShiftScheduleUserRepository();
        public AdminResultModel Check(int userId)
        {
            DateTime now = DateTimeHelper.GetCurrentTime();
            List<TeamShiftScheduleUserModel> checkData =
                TeamShiftScheduleUserRepository.GetCheckDataByFilter(new ShiftScheduleCheckDataFilterParameterModel
                {
                    Hour = 1,
                    Time = now,
                    UserId = userId
                });

            if (checkData.Count == 0)
            {
                return new AdminResultModel
                {
                    ErrorMessage = "你目前不需要簽到",
                    IsSuccess = false
                };
            }
            foreach (var item in checkData)
            {
                //還沒 Checkin 優先 Checkin
                if (item.CheckinTime.HasValue == false)
                {
                    item.CheckinTime = now;
                }
                else
                {
                    item.Checkout = now;
                }
            }
            try
            {
                TeamShiftScheduleUserRepository.Update(checkData);
                return new AdminResultModel
                {
                    IsSuccess = true,
                    ErrorMessage = "成功簽到！"
                };
            }
            catch (Exception ex)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "簽到失敗"
                };
            }
        }
        public List<TeamShiftScheduleViewModel> GetByDepartmentId(int departmentId,int diasterId)
        {
            var result = TeamShiftScheduleRepository.GetDepartmentShiftScheduleByDiasterId(departmentId, diasterId);

            MultiKeyDictionary<int, int, TeamShiftScheduleUserJoinUsersModel> teamScheduleUsers =
                TeamShiftScheduleUserRepository.GetByTeamShiftScheduleIds(result.Select(e => e.Id))
                .ToMultiKeyDictionary(e => e.TeamShiftScheduleId, e => e.Sort, e => e);

            return result.Select(e => 
            {
                TeamShiftScheduleUserJoinUsersModel lader = teamScheduleUsers.GetValue(e.Id, 1);
                return new TeamShiftScheduleViewModel
                {
                    Id = e.Id,
                    LeaderUserId = lader?.UserId,
                    LeaderName = lader?.Name,
                    LeaderUserPhone = lader?.MobilePhone,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime
                };
            }).ToList();
        }

        public void Update(int departmentId, List<TeamShiftScheduleViewModel> models)
        {
            var updateShiftSchedule = models.Select(e => new TeamShiftScheduleModel
            {
                Id = e.Id,
                DepartmentId = departmentId,
                DiasterId = e.DiasterId,
                EndTime = e.EndTime,
                StartTime = e.StartTime
            }).ToList();

            List<TeamShiftScheduleUserModel> teamShiftScheduleUsers = new List<TeamShiftScheduleUserModel>();
            models.ForEach(e => 
            {
                if (e.LeaderUserId.HasValue)
                {
                    teamShiftScheduleUsers.Add(new TeamShiftScheduleUserModel
                    {
                        TeamShiftScheduleId = e.Id,
                        Sort = 1,
                        UserId = e.LeaderUserId.Value
                    });
                }
            });
            TeamShiftScheduleRepository.Update(updateShiftSchedule);
            TeamShiftScheduleUserRepository.DeleteByShiftScheduleIds(updateShiftSchedule.Select(e => e.Id).ToList());
            TeamShiftScheduleUserRepository.Create(teamShiftScheduleUsers);
        }

        public void Moving(int id, int changId)
        {
            var shift = TeamShiftScheduleRepository.Get(id);
            var shift2 = TeamShiftScheduleRepository.Get(changId);

            if(shift == null || shift2 == null)
            {
                return;
            }
            List<TeamShiftScheduleUserModel> shiftScheduleUsers = new List<TeamShiftScheduleUserModel>();

            foreach (var item in TeamShiftScheduleUserRepository.GetByShiftScheduleId(id))
            {
                item.TeamShiftScheduleId = changId;
                shiftScheduleUsers.Add(item);
            }
            foreach (var item in TeamShiftScheduleUserRepository.GetByShiftScheduleId(changId))
            {
                item.TeamShiftScheduleId = id;
                shiftScheduleUsers.Add(item);
            }
            TeamShiftScheduleUserRepository.Update(shiftScheduleUsers);
        }

        public List<ShiftScheduleCheckViewModel> GetSearchCheck(int departmentId, int diasterId)
        {
            var shiftSchedules =
                TeamShiftScheduleRepository.GetDepartmentShiftScheduleByDiasterId(departmentId, diasterId);

            var shiftScheduleUserJoinUsers = TeamShiftScheduleUserRepository.GetByTeamShiftScheduleIds(shiftSchedules.Select(e => e.Id));

            return shiftScheduleUserJoinUsers.Select(e
                => new ShiftScheduleCheckViewModel
                {
                    CheckinTime = e.CheckinTime,
                    Checkout = e.Checkout,
                    Name = e.Name,
                    MobilePhone = e.MobilePhone
                }).ToList();
        }

        public void Delete(int id)
        {
            TeamShiftScheduleRepository.Delete(id);
            TeamShiftScheduleUserRepository.DeleteByShiftScheduleId(id);
        }

        public void Create(int departmentId,int diasterId)
        {
            TeamShiftScheduleRepository.Create(new TeamShiftScheduleModel
            {
                DepartmentId = departmentId,
                DiasterId = diasterId,
                StartTime = DateTimeHelper.GetCurrentTime(),
                EndTime = DateTimeHelper.GetCurrentTime()
            });
        }
    }
}