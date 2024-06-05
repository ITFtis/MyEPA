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

            var file = FileService.GetBySource(SourceTypeEnum.OpenContract, id).FirstOrDefault();

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
                ResourceTypeId = model.ResourceTypeId,
            };
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
                SourceType = SourceTypeEnum.OpenContract,
                User = user.UserName
            });
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

            CheckUserCity(user.CityId, entity.CityId);

            OpenContractRepository.Delete(id);

            OpenContractDetailRepository.DeleteByOpenContractId(id);

            FileService.DeleteFileBySource(SourceTypeEnum.OpenContract, id);

            return new AdminResultModel
            {
                IsSuccess = true
            };
        }

        public void Update(UserBriefModel user, OpenContractViewModel model, HttpPostedFileBase file)
        {
            var entity = OpenContractRepository.Get(model.Id);

            if (entity == null)
                return;

            CheckUserCity(user.CityId, entity.CityId);

            entity.Fac = model.Fac;
            entity.KeyInDate = model.KeyInDate;
            entity.MobileTEL = model.MobileTEL;
            entity.Name = model.Name; 
            entity.OContractDateBegin = model.OContractDateBegin;
            entity.OContractDateEnd = model.OContractDateEnd;
            entity.Owner = model.Owner;
            entity.TEL = model.TEL;
            
            entity.UpdateDate = DateTimeHelper.GetCurrentTime();
            entity.UpdateUser = user.UserName;

            OpenContractRepository.Update(entity);

            if (file != null)
            {
                //刪除檔案
                FileService.DeleteFileBySource(SourceTypeEnum.OpenContract, model.Id);

                //新增檔案
                FileService.UploadFileByGuidName(new UploadFileBaseModel
                {
                    File = file,
                    SourceId = entity.Id,
                    SourceType = SourceTypeEnum.OpenContract,
                    User = user.UserName
                });
            }
        }

        private void CheckUserCity(int userCityId,int cityId)
        {
            if(userCityId != cityId)
            {
                throw new Exception("沒有變更權限");
            }
        }
    }
}