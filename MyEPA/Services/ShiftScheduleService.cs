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
    public class MainShiftScheduleService
    {
        MainShiftScheduleRepository MainShiftScheduleRepository = new MainShiftScheduleRepository();
        MainShiftScheduleUserRepository MainShiftScheduleUserRepository = new MainShiftScheduleUserRepository();

        public AdminResultModel Check(int userId)
        {
            DateTime now = DateTimeHelper.GetCurrentTime();
            List<MainShiftScheduleUserModel> checkData =
                MainShiftScheduleUserRepository.GetCheckDataByFilter(new ShiftScheduleCheckDataFilterParameterModel
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
                MainShiftScheduleUserRepository.Update(checkData);
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
        public List<MainShiftScheduleViewModel> GetByDiasterId(int diasterId)
        {
            var result = MainShiftScheduleRepository.GetShiftScheduleByDiasterId(diasterId);

            MultiKeyDictionary<int, int, MainShiftScheduleUserJoinUsersModel> scheduleUsers =
               MainShiftScheduleUserRepository.GetByShiftScheduleIds(result.Select(e => e.Id))
               .ToMultiKeyDictionary(e => e.MainShiftScheduleId, e => e.Sort, e => e);

            return result.Select(e =>
            {
                MainShiftScheduleUserJoinUsersModel lader = scheduleUsers.GetValue(e.Id, 1);
                MainShiftScheduleUserJoinUsersModel worker = scheduleUsers.GetValue(e.Id, 2);
                return new MainShiftScheduleViewModel
                {
                    Id = e.Id,
                    Date = e.Date,
                    IsNight = e.IsNight,
                    LeaderUserId = lader?.UserId,
                    LeaderUserPhone = lader?.MobilePhone,
                    LeaderDepartmentId = lader?.DepartmentId,
                    WorkerUserId = worker?.UserId,
                    WorkerUserPhone = worker?.MobilePhone,
                    WorkerDepartmentId = worker?.DepartmentId
                };
            }).ToList();
        }

        public List<ShiftScheduleCheckViewModel> GetSearchCheck(int departmentId, int diasterId)
        {
            var shiftSchedules =
               MainShiftScheduleRepository.GetDepartmentShiftScheduleByDiasterId(diasterId);

            var shiftScheduleUserJoinUsers = MainShiftScheduleUserRepository.GetByShiftScheduleIds(shiftSchedules.Select(e => e.Id));

            return shiftScheduleUserJoinUsers.Select(e
                => new ShiftScheduleCheckViewModel
                {
                    CheckinTime = e.CheckinTime,
                    Checkout = e.Checkout,
                    Name = e.Name,
                    MobilePhone = e.MobilePhone
                }).ToList();
        }

        public void Create(int diasterId)
        {
            MainShiftScheduleRepository.Create(new MainShiftScheduleModel
            {
                IsNight = false,
                DiasterId = diasterId,
                Date = DateTimeHelper.GetCurrentTime()
            });
        }
        public void Update(List<MainShiftScheduleViewModel> models)
        {
            var updateShiftSchedule = models.Select(e => new MainShiftScheduleModel
            {
                Id = e.Id,
                IsNight = e.IsNight,
                Date = e.Date,
                DiasterId = e.DiasterId
            }).ToList();
            List<MainShiftScheduleUserModel> shiftScheduleUsers = new List<MainShiftScheduleUserModel>();
            models.ForEach(e =>
            {
                if (e.LeaderUserId.HasValue && e.LeaderDepartmentId.HasValue)
                {
                    shiftScheduleUsers.Add(new MainShiftScheduleUserModel
                    {
                        MainShiftScheduleId = e.Id,
                        Sort = 1,
                        UserId = e.LeaderUserId.GetValueOrDefault(),
                        DepartmentId = e.LeaderDepartmentId.GetValueOrDefault()
                    });
                }
                if (e.WorkerUserId.HasValue && e.WorkerDepartmentId.HasValue)
                {
                    shiftScheduleUsers.Add(new MainShiftScheduleUserModel
                    {
                        MainShiftScheduleId = e.Id,
                        Sort = 2,
                        UserId = e.WorkerUserId.GetValueOrDefault(),
                        DepartmentId = e.WorkerDepartmentId.GetValueOrDefault()
                    });
                }
            });
            MainShiftScheduleRepository.Update(updateShiftSchedule);
            MainShiftScheduleUserRepository.DeleteByMainShiftScheduleIds(updateShiftSchedule.Select(e => e.Id).ToList());
            MainShiftScheduleUserRepository.Create(shiftScheduleUsers);
        }
        public void Delete(int id)
        {
            MainShiftScheduleRepository.Delete(id);
            MainShiftScheduleUserRepository.DeleteByMainShiftScheduleIds(id.ToListCollection());
        }
        public void Moving(int id, int changId)
        {
            var shift = MainShiftScheduleRepository.Get(id);
            var shift2 = MainShiftScheduleRepository.Get(changId);

            if (shift == null || shift2 == null)
            {
                return;
            }
            List<MainShiftScheduleUserModel> shiftScheduleUsers = new List<MainShiftScheduleUserModel>();

            foreach (var item in MainShiftScheduleUserRepository.GetByMainShiftScheduleId(id))
            {
                item.MainShiftScheduleId = changId;
                shiftScheduleUsers.Add(item);
            }
            foreach (var item in MainShiftScheduleUserRepository.GetByMainShiftScheduleId(changId))
            {
                item.MainShiftScheduleId = id;
                shiftScheduleUsers.Add(item);
            }
            MainShiftScheduleUserRepository.Update(shiftScheduleUsers);
        }
    }
    public class ShiftScheduleService
    {
        ShiftScheduleRepository ShiftScheduleRepository = new ShiftScheduleRepository();
        ShiftScheduleUserRepository ShiftScheduleUserRepository = new ShiftScheduleUserRepository();
        public List<AllShiftScheduleQueryModel> QueryAllShiftSchedule(int diasterId,DateTime date,bool? isNight)
        {
            return ShiftScheduleRepository.QueryAllShiftSchedule(diasterId, date, isNight);
        }
        public List<ShiftScheduleViewModel> GetByDepartmentId(int departmentId, int diasterId)
        {
            var result = ShiftScheduleRepository.GetDepartmentShiftScheduleByDiasterId(departmentId, diasterId);

            MultiKeyDictionary<int, int, ShiftScheduleUserJoinUsersModel> scheduleUsers =
                ShiftScheduleUserRepository.GetByShiftScheduleIds(result.Select(e => e.Id))
                .ToMultiKeyDictionary(e => e.ShiftScheduleId, e => e.Sort, e => e);

            return result.Select(e =>
            {
                ShiftScheduleUserJoinUsersModel manager = scheduleUsers.GetValue(e.Id, 0);
                ShiftScheduleUserJoinUsersModel lader = scheduleUsers.GetValue(e.Id, 1);
                ShiftScheduleUserJoinUsersModel worker = scheduleUsers.GetValue(e.Id, 2);
                return new ShiftScheduleViewModel
                {
                    Id = e.Id,
                    IsNight = e.IsNight,
                    LeaderUserId = lader?.UserId,
                    LeaderName = lader?.Name,
                    LeaderUserPhone = lader?.MobilePhone,
                    WorkerUserId = worker?.UserId,
                    WorkerName = worker?.Name,
                    WorkerUserPhone = worker?.MobilePhone,
                    Date = e.Date,
                    ManagerUserId = manager?.UserId,
                    ManagerName = manager?.Name,
                    ManagerUserPhone = manager?.MobilePhone
                };
            }).ToList();
        }

        public AdminResultModel Check(int userId)
        {
            DateTime now = DateTimeHelper.GetCurrentTime();
            List<ShiftScheduleUserModel> checkData =
                ShiftScheduleUserRepository.GetCheckDataByFilter(new ShiftScheduleCheckDataFilterParameterModel
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
                ShiftScheduleUserRepository.Update(checkData);
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

        public void Update(int departmentId, List<ShiftScheduleViewModel> models)
        {
            var updateShiftSchedule = models.Select(e => new ShiftScheduleModel
            {
                Id = e.Id,
                DepartmentId = departmentId,
                IsNight = e.IsNight,
                Date = e.Date,
                DiasterId = e.DiasterId
            }).ToList();

            List<ShiftScheduleUserModel> shiftScheduleUsers = new List<ShiftScheduleUserModel>();

            models.ForEach(e =>
            {
                if (e.ManagerUserId.HasValue)
                {
                    shiftScheduleUsers.Add(new ShiftScheduleUserModel
                    {
                        ShiftScheduleId = e.Id,
                        Sort = 0,
                        UserId = e.ManagerUserId.Value
                    });
                }
                if (e.LeaderUserId.HasValue)
                {
                    shiftScheduleUsers.Add(new ShiftScheduleUserModel
                    {
                        ShiftScheduleId = e.Id,
                        Sort = 1,
                        UserId = e.LeaderUserId.Value
                    });
                }
                if (e.WorkerUserId.HasValue)
                {
                    shiftScheduleUsers.Add(new ShiftScheduleUserModel
                    {
                        ShiftScheduleId = e.Id,
                        Sort = 2,
                        UserId = e.WorkerUserId.Value
                    });
                }
            });

            ShiftScheduleRepository.Update(updateShiftSchedule);
            ShiftScheduleUserRepository.DeleteByShiftScheduleIds(updateShiftSchedule.Select(e => e.Id).ToList());
            ShiftScheduleUserRepository.Create(shiftScheduleUsers);
        }

        public List<ShiftScheduleCheckViewModel> GetSearchCheck(int departmentId, int diasterId)
        {
            var shiftSchedules = 
                ShiftScheduleRepository.GetDepartmentShiftScheduleByDiasterId(departmentId, diasterId);

            var shiftScheduleUserJoinUsers = ShiftScheduleUserRepository.GetByShiftScheduleIds(shiftSchedules.Select(e => e.Id));

            return shiftScheduleUserJoinUsers.Select(e 
                => new ShiftScheduleCheckViewModel 
                {
                    CheckinTime = e.CheckinTime,
                    Checkout = e.Checkout,
                    Name = e.Name,
                    MobilePhone = e.MobilePhone
                }).ToList();
        }

        public void Moving(int id, int changId)
        {
            var shift = ShiftScheduleRepository.Get(id);
            var shift2 = ShiftScheduleRepository.Get(changId);

            if (shift == null || shift2 == null)
            {
                return;
            }
            List<ShiftScheduleUserModel> shiftScheduleUsers = new List<ShiftScheduleUserModel>();

            foreach (var item in ShiftScheduleUserRepository.GetByShiftScheduleId(id))
            {
                item.ShiftScheduleId = changId;
                shiftScheduleUsers.Add(item);
            }
            foreach (var item in ShiftScheduleUserRepository.GetByShiftScheduleId(changId))
            {
                item.ShiftScheduleId = id;
                shiftScheduleUsers.Add(item);
            }
            ShiftScheduleUserRepository.Update(shiftScheduleUsers);
        }

        public void Delete(int id)
        {
            ShiftScheduleRepository.Delete(id);
            ShiftScheduleUserRepository.DeleteByShiftScheduleId(id);
        }

        public void Create(int departmentId, int diasterId)
        {
            ShiftScheduleRepository.Create(new ShiftScheduleModel
            {
                DepartmentId = departmentId,
                IsNight = false,
                DiasterId = diasterId,
                Date = DateTimeHelper.GetCurrentTime()
            });
        }

        public List<UserShiftScheduleCountModel> GetUserShiftScheduleCount(UserShiftScheduleCountFilterModel filter)
        {
            return ShiftScheduleRepository.GetUserShiftScheduleCount(filter);
        }
    }
}