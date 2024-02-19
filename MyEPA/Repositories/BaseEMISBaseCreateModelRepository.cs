using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;

namespace MyEPA.Repositories
{
    public class BaseEMISBaseCreateModelRepository<T> : BaseEMISRepository<T> where T : BaseCreateModel, new()
    {
        public void Create(UserBriefModel user, T model)
        {
            model.CreateDate = DateTimeHelper.GetCurrentTime();
            model.CreateUser = user.UserName;
            base.Create(model);
        }
        public S CreateAndResultIdentity<S>(UserBriefModel user, T model)
        {
            model.CreateDate = DateTimeHelper.GetCurrentTime();
            model.CreateUser = user.UserName;
            return base.CreateAndResultIdentity<S>(model);
        }
    }
}