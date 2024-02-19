using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Models.SearchViewModel;
using MyEPA.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class ContactManualTeamService : ContactManualBaseService
    {
        ContactManualRoleRepository ContactManualRoleRepository = new ContactManualRoleRepository();
        UsersRepository UsersRepository = new UsersRepository();
        ContactManualDepartmentRepository ContactManualDepartmentRepository = new ContactManualDepartmentRepository();
        public List<ContactManualTeamViewModel> GetList(ContactManualTypeEnum type)
        {
            IEnumerable<ContactManualModel> result =
                ContactManualRepository.GetListByFilter(new ContactManualFilterParameter
                {
                    Types = type.ToListCollection()
                }).OrderBy(e => e.Sort);
            return ConvertToViewModel(result);
        }
        private List<ContactManualTeamViewModel> ConvertToViewModel(IEnumerable<ContactManualModel> models)
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

                return new ContactManualTeamViewModel
                {
                    HomeNumber = user?.HomeNumber,
                    Id = e.Id,
                    MobilePhone = user?.MobilePhone,
                    Name = user?.Name,
                    PositionName = user?.PositionName,
                    OfficePhone = officePhone,
                    Extension = extension,
                    FaxNumber = user?.FaxNumber,
                    Note = user?.Remark,
                    RoleName = roles.GetValue(e.RoleId),
                    DepartmentName = depa.GetValue(e.SourceId),
                    Sort = e.Sort
                };
            }).ToList();
        }
    }
}