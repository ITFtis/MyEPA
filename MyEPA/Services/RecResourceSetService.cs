using MyEPA.Models;
using MyEPA.Repositories;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Services
{
    public class RecResourceSetService : BaseEMISRepository<RecResourceSetModel>
    {
        RecResourceSetRepository RecResourceSetRepository = new RecResourceSetRepository();

        public void Create(UserBriefModel user, RecResourceSetModel model)
        {
            model.CreateUser = user.UserName;
            model.CreateDate = DateTime.Now;

            RecResourceSetRepository.Create(model);
        }
    }
}