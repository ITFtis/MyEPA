using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;

namespace MyEPA.Repositories
{
    public class BaseEMISBaseModelRepository<T> : BaseEMISRepository<T> where T : BaseModel, new()
    {
        public void Create(UserBriefModel user, T model)
        {
            model.CreateDate = DateTimeHelper.GetCurrentTime();
            model.CreateUser = user.UserName;
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateUser = user.UserName;
            base.Create(model);
        }
        public S CreateAndResultIdentity<S>(UserBriefModel user, T model)
        {
            model.CreateDate = DateTimeHelper.GetCurrentTime();
            model.CreateUser = user.UserName;
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateUser = user.UserName;
            return base.CreateAndResultIdentity<S>(model);
        }
        public void Update(UserBriefModel user, T model)
        {
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateUser = user.UserName;
            base.Update(model);
        }
    }
}