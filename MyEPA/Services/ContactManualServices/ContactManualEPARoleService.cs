using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class ContactManualEPARoleService : ContactManualBaseService
    {
        UsersRepository UsersRepository = new UsersRepository();
        ContactManualRoleRepository ContactManualRoleRepository = new ContactManualRoleRepository();
        public List<ContactManualEPARoleViewModel> GetList(ContactManualTypeEnum typeEnum)
        {
            List<ContactManualModel> models = ContactManualRepository.GetListByFilter(new ContactManualFilterParameter
            {
                Types = typeEnum.ToListCollection()
            });

            return ConvertToViewModel(models);
        }
        private List<ContactManualEPARoleViewModel> ConvertToViewModel(List<ContactManualModel> models)
        {
            if (models.IsEmptyOrNull())
            {
                return new List<ContactManualEPARoleViewModel>();
            }
            var users = UsersRepository.GetUsersInfoByFilter(new UsersInfoFilterParameter
            {
                UserIds = models.Select(e => e.UserId)
            }).ToDictionary(e => e.Id, e => e);

            var roles = ContactManualRoleRepository.GetByFilter(new ContactManualRoleFilterParameter
            {
                Ids = models.Select(e => e.RoleId).ToList()
            }).ToDictionary(e => e.Id, e => e);

            return models.Select(e =>
            {
                var user = users.GetValue(e.UserId);

                string[] officePhoneData = (user?.OfficePhone ?? string.Empty).Split('#');
                string officePhone = officePhoneData.Length > 0 ? officePhoneData[0] : string.Empty;
                string extension = officePhoneData.Length > 1 ? officePhoneData[1] : string.Empty;

                return new ContactManualEPARoleViewModel
                {
                    SourceId = e.SourceId,
                    HomeNumber = user?.HomeNumber,
                    Id = e.Id,
                    MobilePhone = user?.MobilePhone,
                    Name = user?.Name,
                    PositionName = user?.PositionName,
                    OfficePhone = officePhone,
                    Extension = extension,
                    FaxNumber = user?.FaxNumber,
                    Note = e.Note,
                    RoleName = roles.GetValue(e.RoleId)?.Name,
                    Sort = e.Sort
                };
            }).ToList();
        }
    }
}