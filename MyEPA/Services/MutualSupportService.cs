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
    public class MutualSupportService
    {
        MutualSupportRepository MutualSupportRepository = new MutualSupportRepository();
        FileDataService FileService = new FileDataService();
        ResourceTypeRepository ResourceTypeRepository = new ResourceTypeRepository();
        public List<MutualSupportModel> GetByFilter(MutualSupportFilterParameter filter)
        {
            return MutualSupportRepository.GetByFilter(filter);
        }

        public void Create(UserBriefModel user, MutualSupportModel model, HttpPostedFileBase file)
        {
            
            model.CreateDate = DateTimeHelper.GetCurrentTime();
            model.CreateUser = user.UserName;
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateUser = user.UserName;

            var id = MutualSupportRepository.CreateAndResultIdentity<int>(model);

            FileService.UploadFileByGuidName(new UploadFileBaseModel
            {
                File = file,
                SourceId = id,
                SourceType = SourceTypeEnum.MutualSupport,
                User = user.UserName
            });
        }

        public MutualSupportViewModel Get(int id)
        {
            var model = MutualSupportRepository.Get(id);

            if (model == null)
            {
                return null;
            }

            var file = FileService.GetBySource(SourceTypeEnum.MutualSupport, id).FirstOrDefault();

            return new MutualSupportViewModel
            {
                AcceptSection = model.AcceptSection,
                EndDate = model.EndDate,
                FileData = file,
                Id = model.Id,
                Memo = model.Memo,
                ResourceTypeId = model.ResourceTypeId,
                Section = model.Section,
                StartDate = model.StartDate,
                SupportContent = model.SupportContent,
                SupportType = model.SupportType
            };
        }

        public void Update(UserBriefModel user, MutualSupportViewModel model, HttpPostedFileBase file)
        {
            var entity = MutualSupportRepository.Get(model.Id);

            if (entity == null)
                return;

            entity.AcceptSection = model.AcceptSection;
            entity.EndDate = model.EndDate;
            entity.Memo = model.Memo;
            entity.ResourceTypeId = model.ResourceTypeId;
            entity.Section = model.Section;
            entity.StartDate = model.StartDate;
            entity.SupportContent = model.SupportContent;
            entity.SupportType = model.SupportType;

            entity.UpdateDate = DateTimeHelper.GetCurrentTime();
            entity.UpdateUser = user.UserName;

            MutualSupportRepository.Update(entity);

            if (model.FileData == null || file != null)
            {
                FileService.DeleteFileBySource(SourceTypeEnum.MutualSupport, model.Id);
            }

            FileService.UploadFileByGuidName(new UploadFileBaseModel
            {
                File = file,
                SourceId = entity.Id,
                SourceType = SourceTypeEnum.MutualSupport,
                User = user.UserName
            });
        }

        public AdminResultModel Delete(int id)
        {
            var entity = MutualSupportRepository.Get(id);

            if (entity == null)
                return new AdminResultModel 
                {
                    IsSuccess = false,
                    ErrorMessage = "資料不存在"
                };
            try
            {
                MutualSupportRepository.Delete(id);

                FileService.DeleteFileBySource(SourceTypeEnum.MutualSupport, id);
            }
            catch(Exception ex)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
            return new AdminResultModel
            {
                IsSuccess = true
            };
        }

        public List<MutualSupportSearchViewModel> Search(MutualSupportFilterParameter filter)
        {
            List<MutualSupportModel> query = MutualSupportRepository.GetByFilter(filter);

            Dictionary<int, string> resourceTypes =
                ResourceTypeRepository.GetList()
                .ToDictionary(e => e.Id, e => e.Name);

            return query.Select(e => new MutualSupportSearchViewModel
            {
                AcceptSection = e.AcceptSection,
                EndDate = e.EndDate,
                Id = e.Id,
                Memo = e.Memo,
                Section = e.Section,
                StartDate = e.StartDate,
                SupportContent = e.SupportContent,
                SupportType = e.SupportType,
                ResourceTypeId = e.ResourceTypeId,
                SupportTypeName = ((SupportTypeEnum)e.SupportType).GetDescription(),
                ResourceType = resourceTypes.GetValue(e.ResourceTypeId),
            }).ToList();
        }
    }
}