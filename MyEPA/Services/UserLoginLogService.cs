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

        /// <summary>
        /// 清除密碼輸入錯誤的Log
        /// </summary>
        /// <param name="UserName">帳號</param>
        /// <param name="lockTime">15(分)</param>
        /// <returns></returns>
        public bool UpdateIsOver(string userName, int lockTime = 0)
        {
            bool done = UserLoginLogRepository.UpdateIsOver(userName, lockTime);
            return true;
        }
    }
}