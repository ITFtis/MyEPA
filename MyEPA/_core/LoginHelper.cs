using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA
{
    public class LoginHelper
    {
        /// <summary>
        /// 鎖定上限次數
        /// </summary>
        public const int lockUp = 3;

        /// <summary>
        /// 鎖定時間(分)
        /// </summary>
        public const int lockTime = 15;

        /// <summary>
        /// 累積登入錯誤次數
        /// </summary>
        /// <returns></returns>
        public static int LoginCount(string account)
        {
            List<InvalidLogin> user = null;
            if (HttpContext.Current.Application["Users"] == null) //Adding List to Application State
            {
                user = new List<InvalidLogin>();
            }
            else
            {
                user = (List<InvalidLogin>)HttpContext.Current.Application["Users"];
            }
            var remove = user.RemoveAll(x => x.Attempttime < DateTime.Now.AddMinutes(-1 * lockTime));//Remove IP Before 15 minutes(Give 15 Min Time Next Login)
            var checkLogged = user.Find(x => x.Account == account);
            if (checkLogged == null)
            {
                checkLogged = new InvalidLogin
                {
                    Account = account,
                    Attempttime = DateTime.Now,
                    AttemptCount = 1

                };

                user.Add(checkLogged);
                HttpContext.Current.Application["Users"] = user;
            }
            else
            {
                if (checkLogged.AttemptCount < (lockUp + 1))
                {
                    checkLogged.Attempttime = DateTime.Now;
                    checkLogged.AttemptCount++;
                    HttpContext.Current.Application["Users"] = user;
                }
            }

            return checkLogged.AttemptCount;
        }

        /// <summary>
        /// 紀錄登入失敗資料
        /// </summary>
        public class InvalidLogin
        {
            /// <summary>
            /// Account
            /// </summary>
            public string Account { get; set; }
            /// <summary>
            /// 登入失敗時間
            /// </summary>
            public DateTime Attempttime { get; set; }
            /// <summary>
            /// 登入失敗次數
            /// </summary>
            public int AttemptCount { get; set; }
        }
    }
}