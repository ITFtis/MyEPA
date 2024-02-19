using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MyEPA.Services
{
    public class ContactManualOnDutyService 
    {
        ContactManualRepository ContactManualRepository = new ContactManualRepository();
        ContactManualDateRepository ContactManualDateRepository = new ContactManualDateRepository();
        UsersRepository UsersRepository = new UsersRepository();
        /// <summary>
        /// 監資處春節期間空氣品質污染指標預報值班人員通聯表
        /// </summary>
        /// <param name="typeEnum"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public List<ContactManualOnDutyViewModel> GetListByOnDutyType(ContactManualTypeEnum type)
        {
            var models = ContactManualRepository.GetListByFilter(new ContactManualFilterParameter
            {
                Types = type.ToListCollection(),
                SourceIds = ContactManualOnDutyTypeEnum.SpringFestival.ToInteger().ToListCollection()
            }).OrderBy(e => e.Sort);
           
            return ConvertToViewModel(models);
        }

        public void Create(UserBriefModel user, ContactManualOnDutyCreateViewModel model)
        {
            var id = ContactManualRepository.CreateAndResultIdentity<int>(user,new ContactManualModel 
            {
                Note = model.Note,
                Sort = model.Sort,
                SourceId = ContactManualOnDutyTypeEnum.SpringFestival.ToInteger(),
                Type = model.Type,
                UserId = model.UserId
            });

            ContactManualDateRepository.Create(new ContactManualDateModel
            {
                Id = id,
                Date = model.Date
            });
        }

        public void Delete(int id)
        {
            ContactManualRepository.Delete(id);
            ContactManualDateRepository.Delete(id);
        }

        private List<ContactManualOnDutyViewModel> ConvertToViewModel(IEnumerable<ContactManualModel> models)
        {
            var users = UsersRepository.GetUsersInfoByFilter(new UsersInfoFilterParameter
            {
                UserIds = models.Select(e => e.UserId)
            }).ToDictionary(e => e.Id, e => e);

            var contactManualDates = 
                ContactManualDateRepository.GetList()
                .ToDictionary(e => e.Id, e => e.Date);

            CultureInfo.CreateSpecificCulture("zh-tw");

            return models.Select(e =>
            {
                var user = users.GetValue(e.UserId);
                var date = contactManualDates.GetValue(e.Id);
                return new ContactManualOnDutyViewModel
                {
                    Id = e.Id,
                    Date = date,
                    HomeNumber = user?.HomeNumber,
                    MobilePhone = user?.MobilePhone,
                    Name = user?.Name,
                    Week = date.ToString("ddd"),
                    Sort = e.Sort
                };
            }).ToList();
        }
    }
}