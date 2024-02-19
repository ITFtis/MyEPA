using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class ContactManualService : ContactManualBaseService
    {
        UsersRepository UsersRepository = new UsersRepository();
        ContactManualDepartmentRepository ContactManualDepartmentRepository = new ContactManualDepartmentRepository();
        PositionRepository PositionRepository = new PositionRepository();
        public List<ContactManualViewModel> GetListByType(ContactManualTypeEnum typeEnum)
        {
            var models = ContactManualRepository.GetListByType(typeEnum).OrderBy(e => e.Sort);
            return ConvertToViewModel(models);
        }

        public List<ContactManualViewModel> GetListBySourceId(ContactManualTypeEnum typeEnum,int sourceId)
        {
            var models = ContactManualRepository.GetListByFilter(new ContactManualFilterParameter
            {
                Types = typeEnum.ToListCollection(),
                SourceIds = sourceId.ToListCollection()
            }).OrderBy(e => e.Sort);

            return ConvertToViewModel(models);
        }

        
        private List<ContactManualViewModel> ConvertToViewModel(IEnumerable<ContactManualModel> models)
        {
            var users = UsersRepository.GetListByFilter(new UsersFilterParameter
            {
                UserIds = models.Select(e => e.UserId)
            });

            var userDic = users.ToDictionary(e => e.Id, e => e);

            var departments = ContactManualDepartmentRepository.GetListByFilter(new ContactManualDepartmentParameter
            {
                Ids = models.Select(e => e.SourceId).ToHashSet()
            }).ToDictionary(e => e.Id, e => e.Name);

            var positions = PositionRepository.GetList().ToDictionary(e => e.Id, e => e.Name);

            return models.Select(contactManual =>
            {
                UsersModel user = userDic.GetValue(contactManual.UserId);
                string positionName = user == null ? string.Empty : positions.GetValue(user.PositionId);
                string[] officePhoneData = (user?.OfficePhone ?? string.Empty).Split('#');
                string officePhone = officePhoneData.Length > 0 ? officePhoneData[0] : string.Empty;
                string extension = officePhoneData.Length > 1 ? officePhoneData[1] : string.Empty;
                string departmentName = departments.GetValue(contactManual.SourceId);

                return new ContactManualViewModel
                {
                    HomeNumber = user?.HomeNumber,
                    Id = contactManual.Id,
                    MobilePhone = user?.MobilePhone,
                    Name = user?.Name,
                    PositionName = positionName,
                    OfficePhone = officePhone,
                    Extension = extension,
                    FaxNumber = user?.FaxNumber,
                    Note = contactManual.Note,
                    DepartmentName = departmentName,
                    Sort = contactManual.Sort,
                    SourceId = contactManual.SourceId
                };
            }).ToList();
        }
    }
}