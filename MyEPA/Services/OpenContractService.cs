using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Services
{
    public class OpenContractService
    {
        private string _errorMessage = "";

        public string ErrorMessage { get { return _errorMessage; } }

        OpenContractRepository OpenContractRepository = new OpenContractRepository();
        OpenContractDetailRepository OpenContractDetailRepository = new OpenContractDetailRepository();
        FileDataService FileService = new FileDataService();

        public List<OpenContractJoinDetailSearchModel> Search(OpenContractFilterParameter filter)
        {
            var result = OpenContractRepository.GetJoinDetailsByFilter(filter);

            return result;
        }
        public List<OpenContractModel> GetListByFilter(OpenContractFilterParameter filter)
        {
            return OpenContractRepository.GetByFilter(filter);
        }
        public OpenContractViewModel Get(int id)
        {
            var model = OpenContractRepository.Get(id);
            
            if(model == null)
            {
                return null;
            }

            var file = FileService.GetBySource(SourceTypeEnum.OpenContractCover, id).FirstOrDefault();

            return new OpenContractViewModel
            {
                Fac = model.Fac,
                FileData = file,
                Id = model.Id,
                KeyInDate = model.KeyInDate,
                MobileTEL = model.MobileTEL,
                Name = model.Name,
                OContractDateBegin = model.OContractDateBegin,
                OContractDateEnd = model.OContractDateEnd,
                Owner = model.Owner,
                TEL = model.TEL,
                CityId = model.CityId,
                TownId = model.TownId,
                IsSupportCity = model.IsSupportCity,
                ResourceTypeId = model.ResourceTypeId,
            };
        }

        /// <summary>
        /// 有細目數量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<OpenContractCountModel> GetCountListByFilter(OpenContractFilterParameter filter)
        {
            return OpenContractRepository.GetCountByFilter(filter);
        }

        public void Create(UserBriefModel user, OpenContractModel model, HttpPostedFileBase file)
        {
            model.CreateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.CreateUser = user.UserName;
            model.UpdateUser = user.UserName;
            model.CityId = user.CityId;
            model.TownId = user.TownId;

            var id = OpenContractRepository.CreateAndResultIdentity<int>(model);

            FileService.UploadFileByGuidName(new UploadFileBaseModel
            {
                File = file,
                SourceId = id,
                SourceType = SourceTypeEnum.OpenContractCover,
                User = user.UserName
            });
        }

        /// <summary>
        /// 複製來源主約Id
        /// </summary>
        /// <param name="user"></param>
        /// <param name="copyId"></param>
        /// <returns>新建置主約Id</returns>
        public int CopyOpenContractById(UserBriefModel user, int copyId)
        {
            int id = OpenContractRepository.CopyOpenContractById(user, copyId);
            return id;
        }

        public AdminResultModel Delete(UserBriefModel user, int id)
        {
            var entity = OpenContractRepository.Get(id);

            if (entity == null)
                return new AdminResultModel 
                {
                    IsSuccess = false,
                    ErrorMessage = "資料不存在"
                };

            if (!CheckPermissions(user, entity.CityId, entity.TownId))
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = _errorMessage,
                };
            }

            OpenContractRepository.Delete(id);

            OpenContractDetailRepository.DeleteByOpenContractId(id);

            FileService.DeleteFileBySource(SourceTypeEnum.OpenContractCover, id);

            return new AdminResultModel
            {
                IsSuccess = true
            };
        }

        public bool Update(UserBriefModel user, OpenContractViewModel model, HttpPostedFileBase file)
        {
            bool result = false;

            var entity = OpenContractRepository.Get(model.Id);

            if (entity == null)
                return false;

            //確認是否有權限
            if (!CheckPermissions(user, entity.CityId, entity.TownId))
            {
                return false;
            }

            entity.Fac = model.Fac;
            entity.KeyInDate = model.KeyInDate;
            entity.MobileTEL = model.MobileTEL;
            entity.ResourceTypeId = model.ResourceTypeId;
            entity.Name = model.Name; 
            entity.OContractDateBegin = model.OContractDateBegin;
            entity.OContractDateEnd = model.OContractDateEnd;
            entity.Owner = model.Owner;
            entity.TEL = model.TEL;
            entity.IsSupportCity = model.IsSupportCity;

            //-1:Copy主約未修改,0:一般資料
            entity.Status = 0;
            if (entity.CreateDate == DateTime.MinValue)
            {
                entity.CreateDate = DateTime.Now;
                entity.CreateUser = user.UserName;
            }

            entity.UpdateDate = DateTimeHelper.GetCurrentTime();
            entity.UpdateUser = user.UserName;

            OpenContractRepository.Update(entity);

            if (file != null)
            {
                //刪除檔案
                FileService.DeleteFileBySource(SourceTypeEnum.OpenContractCover, model.Id);

                //新增檔案
                FileService.UploadFileByGuidName(new UploadFileBaseModel
                {
                    File = file,
                    SourceId = entity.Id,
                    SourceType = SourceTypeEnum.OpenContractCover,
                    User = user.UserName
                });
            }
        
            result = true;

            return result;
        }

        /// <summary>
        /// 確認是否有權限
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public bool CheckPermissions(UserBriefModel user, int cityId, int townId)
        {
            bool result = true;

            if (user.CityId != cityId)
            {
                _errorMessage = "沒有變更權限(縣市不符)";
                result = false;
            }
            else if (user.TownId != townId)
            {
                _errorMessage = "沒有變更權限(鄉鎮不符)";
                result = false;
            }
            return result;
        }
    }
}