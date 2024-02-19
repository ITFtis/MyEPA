using MyEPA.Models;
using MyEPA.Repositories;
using MyEPA.ViewModels;

namespace MyEPA.Services
{
    public class ContactManualBaseService
    {
        protected ContactManualRepository ContactManualRepository = new ContactManualRepository();
        public void Delete(int id)
        {
            ContactManualRepository.Delete(id);
        }

        public void Create(UserBriefModel user, ContactManualEPBViewModel model)
        {
            ContactManualRepository.Create(user, new ContactManualModel
            {
                SourceId = model.CityId,
                Type = model.Type,
                UserId = model.UserId,
                Note = model.Note,
                RoleId = model.RoleId,
                Sort = model.Sort
            });
        }
        public void Create(UserBriefModel user, ContactManualEPAViewModel model)
        {
            ContactManualRepository.Create(user, new ContactManualModel
            {
                SourceId = model.DepartmentId.GetValueOrDefault(),
                Type = model.Type,
                UserId = model.UserId,
                RoleId = model.RoleId,
                Sort = model.Sort,
                Note = model.Note
            });
        }
        public void Create(UserBriefModel user, ContactManualModel model)
        {
            ContactManualRepository.Create(user, model);
        }
    }
}