using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Models.SearchViewModel;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MyEPA.Services
{
    public class ContactManualRecycleService : ContactManualBaseService
    {
        UsersRepository UsersRepository = new UsersRepository();
        ContactManualRoleRepository ContactManualRoleRepository = new ContactManualRoleRepository();
        ContactManualDepartmentRepository ContactManualDepartmentRepository = new ContactManualDepartmentRepository();

        public List<ContactManualRecycleViewModel> GetList(ContactManualTypeEnum type)
        {
            var result =
                ContactManualRepository.GetListByFilter(new ContactManualFilterParameter
                {
                    Types = type.ToListCollection()
                }).OrderBy(e => e.Sort);
            return ConvertToViewModel(result);
        }
        public List<ContactManualRecycleReportModel> GetReportList(ContactManualTypeEnum type)
        {
            var result =
                ContactManualRepository.GetListByFilter(new ContactManualFilterParameter
                {
                    Types = type.ToListCollection()
                }).OrderBy(e => e.Sort);
            return ConvertToViewModel(result).ConvertToModel<ContactManualRecycleViewModel, ContactManualRecycleReportModel>();
        }
        private List<ContactManualRecycleViewModel> ConvertToViewModel(IEnumerable<ContactManualModel> models)
        {
            var users = UsersRepository.GetUsersInfoByFilter(new UsersInfoFilterParameter
            {
                UserIds = models.Select(e => e.UserId)
            }).ToDictionary(e => e.Id, e => e);

            var roles = ContactManualRoleRepository.GetByFilter(new ContactManualRoleFilterParameter
            {
                Ids = models.Select(e => e.RoleId).ToList()
            }).ToDictionary(e => e.Id, e => e.Name);

            var depa = ContactManualDepartmentRepository.GetListByFilter(new ContactManualDepartmentParameter
            {
                Ids = models.Select(e => e.SourceId).ToList()
            }).ToDictionary(e => e.Id, e => e.Name);

            return models.Select(e =>
            {
                var user = users.GetValue(e.UserId);

                string[] officePhoneData = (user?.OfficePhone ?? string.Empty).Split('#');
                string officePhone = officePhoneData.Length > 0 ? officePhoneData[0] : string.Empty;
                string extension = officePhoneData.Length > 1 ? officePhoneData[1] : string.Empty;

                return new ContactManualRecycleViewModel
                {
                    HomeNumber = user?.HomeNumber,
                    Id = e.Id,
                    MobilePhone = user?.MobilePhone,
                    Name = user?.Name,
                    PositionName = user?.PositionName,
                    OfficePhone = officePhone,
                    Extension = extension,
                    FaxNumber = user?.FaxNumber,
                    Note = e.Note,
                    RoleName = roles.GetValue(e.RoleId),
                    DepartmentName = depa.GetValue(e.SourceId),
                    Sort = e.Sort
                    
                };
            }).ToList();
        }
    }
}