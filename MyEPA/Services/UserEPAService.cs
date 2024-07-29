using MyEPA.Enums;
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
    public class UserEPAService
    {
        CityRepository CityRepository = new CityRepository();
        UsersRepository UsersRepository = new UsersRepository();
        PositionRepository PositionRepository = new PositionRepository();
        ContactManualDepartmentRepository ContactManualDepartmentRepository = new ContactManualDepartmentRepository();

        public List<UserEPAListViewModel> GetListByFilter(UsersFilterParameter usersFilter)
        {
            List<UsersModel> users = UsersRepository.GetListByFilter(usersFilter);

            var positions = PositionRepository.GetList().ToDictionary(e => e.Id, e => e.Name);
            var departments = ContactManualDepartmentRepository.GetList().ToDictionary(e => e.Id, e => e.Name);

            return users.ConvertToModel<UsersModel, UserEPAListViewModel>((input, outPut) =>
            {
                if (input.ContactManualDepartmentId.HasValue)
                {
                    outPut.DepartmentName = departments.GetValue(input.ContactManualDepartmentId.Value);
                }
                outPut.PositionName = positions.GetValue(input.PositionId);
            }).ToList();
        }
        public PagingResult<UserEPAListViewModel> GetPagingList(UsersFilterPaginationParameter usersFilter)
        {
            PagingResult<UsersModel> users = UsersRepository.GetPageingByFilter(usersFilter);

            var positions = PositionRepository.GetList().ToDictionary(e => e.Id, e => e.Name);
            var departments = ContactManualDepartmentRepository.GetList().ToDictionary(e => e.Id, e => e.Name);

            var result = new PagingResult<UserEPAListViewModel>
            {
                Items = users.Items.ConvertToModel<UsersModel, UserEPAListViewModel>((input, outPut) =>
                {
                    if (input.ContactManualDepartmentId.HasValue)
                    {
                        outPut.DepartmentName = departments.GetValue(input.ContactManualDepartmentId.Value);
                    }
                    outPut.PositionName = positions.GetValue(input.PositionId);
                }).ToList(),
                Pagination = users.Pagination
            };

            return result;
        }

        public UserEPAViewModel Get(int id)
        {
            return UsersRepository.Get(id).ConvertToModel<UsersModel, UserEPAListViewModel>((input,output)=> 
            {
                output.DepartmentId = input.ContactManualDepartmentId;
            });
        }
        public void Update(UserEPAViewModel input)
        {
            var model = UsersRepository.Get(input.Id);
            model.Name = input.Name;
            model.MobilePhone = input.MobilePhone;
            model.OfficePhone = input.OfficePhone;
            model.FaxNumber = input.FaxNumber;
            model.Email = input.Email;
            model.Remark = input.Remark;
            model.TownId = input.TownId;
            model.PositionId = input.PositionId;
            model.HomeNumber = input.HomeNumber;
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.ContactManualDuty = input.ContactManualDuty;
            model.ContactManualDepartmentId = input.DepartmentId;
            UsersRepository.Update(model);
        }

        public void Create(UserEPAViewModel model)
        {
            CityModel city = CityRepository.GetByCityName("環保署");

            UsersRepository.Create(new UsersModel
            {
                IsAdmin = false,
                City = city.City,
                CityId = city.Id,
                ConfirmTime = null,
                ContactManualDepartmentId = model.DepartmentId,
                Duty = DutyEnum.EPA.GetDescription(),
                DutyId = DutyEnum.EPA.ToInteger(),
                Email = model.Email,
                FaxNumber = model.FaxNumber,
                HomeNumber = model.HomeNumber,
                HumanType = string.Empty,
                MainContacter = "否",
                MobilePhone = model.MobilePhone,
                Name = model.Name,
                OfficePhone = model.OfficePhone,
                PositionId = model.PositionId,
                Pwd = model.MobilePhone,
                PwdUpdateDate = DateTime.Now.AddDays(90),
                Remark = model.Remark,
                ReportPriority = "",
                UpdateDate = DateTimeHelper.GetCurrentTime(),
                UserName = model.UserName,
                VoicePwd = string.Empty,
                ContactManualDuty = ContactManualDutyEnum.User
            });
        }
    }
}