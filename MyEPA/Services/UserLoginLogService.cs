using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Services
{
    public class UserLoginLogService
    {
        UserLoginLogRepository UserLoginLogRepository = new UserLoginLogRepository();

        public UserLoginLogModel Get(int Serial)
        {
            return UserLoginLogRepository.Get(Serial);
        }

        public List<UserLoginLogModel> GetListByFilter(UserLoginLogFilterParameter filter)
        {
            return UserLoginLogRepository.GetListByFilter(filter);
        }

        public void Create(UserLoginLogModel model)
        {
            UserLoginLogRepository.Create(model);
        }
    }
}