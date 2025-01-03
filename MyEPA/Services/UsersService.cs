using DocumentFormat.OpenXml.Spreadsheet;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Services
{
    public class UsersService
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        UsersRepository UsersRepository = new UsersRepository();
        public PagingResult<UsersViewModel> GetPagingList(UsersFilterPaginationParameter usersFilter)
        {
            PagingResult<UsersModel> users = UsersRepository.GetPageingByFilter(usersFilter);

            return new PagingResult<UsersViewModel> 
            {
                Items = ConvertToViewModel(users.Items),
                Pagination = users.Pagination
            };
        }
        private List<UsersViewModel> ConvertToViewModel(IEnumerable<UsersModel> models)
        {
            var positions = new PositionRepository().GetList().ToDictionary(e => e.Id, e => e.Name);

            return models.Select(user =>
            {
                string positionName = user == null ? string.Empty : positions.GetValue(user.PositionId);

                return new UsersViewModel
                {
                    Id = user.Id,
                    City = user.City,
                    Duty = user.Duty,
                    HumanType = user.HumanType,
                    Name = user.Name,
                    PositionName = positionName,
                    Town = user.Town,
                    UpdateDate = user.UpdateDate
                };
            }).ToList();
        }
        public UsersModel GetByUserName(string userName)
        {
            return UsersRepository.GetByUserName(userName);
        }
        public List<UsersModel> GetAll()
        {
            return UsersRepository.GetList();
        }

        public List<UsersBriefModel> GetListBriefByFilter(UsersBriefFilterParameter filter)
        {
            return UsersRepository.GetListBriefByFilter(filter);
        }
        public UserEditViewModel GetEditById(int id)
        {
            UserEditViewModel result = UsersRepository.Get(id).ConvertToModel<UsersModel, UserEditViewModel>();
            return result;
        }
        public UsersModel GetById(int id)
        {
            return UsersRepository.Get(id);
        }

        public List<UsersModel> GetByDepartmentId(int departmentId)
        {
            return UsersRepository.GetUsersJoinPositionByFilter(new UsersJoinPositionFilterParameter 
            {
                DepartmentIds = departmentId.ToListCollection()
            });
        }

        public List<UsersModel> GetUsersJoinPositionByFilter(UsersJoinPositionFilterParameter filter)
        {
            return UsersRepository.GetUsersJoinPositionByFilter(filter);
        }

        /// <summary>
        /// 聯絡人未登入
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<UserLoginViewModel> GetUserLoginByFilter(UsersFilterParameter filter)
        {
            var model = UsersRepository.GetUserLoginByFilter(filter);

            return model;
        }

        public AdminResultModel UpdatePwd(int id,UsersEditPwdViewModel model)
        {
            if (!PwdHelper.ValidPassword(model.Pwd))
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = PwdHelper.ErrorMessage
                };
            };

            UsersModel users = UsersRepository.Get(id);
            if (model.OldPwd != users.Pwd)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "原密碼輸入錯誤"
                };
            }
            else if (model.Pwd == users.Pwd)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "密碼不可與前次重複"
                };
            }

            users.Pwd = model.Pwd;
            users.PwdUpdateDate = DateTime.Now.AddDays(90);
            UsersRepository.Update(users);

            //更新密碼資料
            HttpContext.Current.Session["PwdUpdateDate"] = users.PwdUpdateDate;

            //DEDS 同步修改密碼
            DedsUpdarePwd(users.UserName, users.Pwd);

            return new AdminResultModel
            {
                IsSuccess = true                
            };
        }
        public void Updare(UserModel user)
        {
            var entity = UsersRepository.Get(user.Id);


            UsersRepository.Update(entity);
        }
        public void Delete(int id)
        {
            UsersRepository.Delete(id);
        }

        /// <summary>
        /// DEDS 同步修改密碼
        /// </summary>
        /// <param name="id">帳號</param>
        /// <param name="password">密碼</param>
        public bool DedsUpdarePwd(string id, string password)
        {
            bool result = false;

            try
            {
                using (var dbDEDS = new DedsModelContext())
                {
                    var vs = dbDEDS.User.Where(a => a.Id == id);
                    if (vs.Count() == 1)
                    {
                        var v = vs.First();
                        v.Password = password;

                        dbDEDS.SaveChanges();
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                logger.Error("DEDS 同步修改密碼");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }

        public List<UsersInfoModel> GetUsersInfoByFilter(UsersInfoFilterParameter filter)
        {
            return UsersRepository.GetUsersInfoByFilter(filter);
        }

        public List<UsersModel> GetUsersByFilter(UsersBriefFilterParameter filter)
        {
            return UsersRepository.GetListByFilter(filter);
        }

        public UsersModel GetUserByUserNameAndPwd(string username, string pwd)
        {
            return UsersRepository.GetUserByUserNameAndPwd(username, pwd);
        }
        public UsersModel GetUserByUserNameAndemail(string username, string email)
        {
            return UsersRepository.GetUserByUserNameAndemail(username, email);
        }
        public void AddUserLoginLog(UsersModel user)
        {
             UsersRepository.AddUserLoginLog(user);
        }
        public void AddUserEmail(UsersModel user, string email)
        {
            UsersRepository.AddUserEmail(user, email);
        }
    }
}