using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Web;

namespace MyEPA.Services
{

    public class PhoneWorkService
    {
        PhoneWorkRepository PhoneWorkRepository = new PhoneWorkRepository();
        public List<PhoneWorkModel> GetAll()
        {
            return PhoneWorkRepository.GetList();
        }

        public PhoneWorkViewModel Get(int id)
        {
            var phoneWork = PhoneWorkRepository.Get(id);
            return new PhoneWorkViewModel 
            {
                CreateDate = phoneWork.CreateDate,
                CreateUser = phoneWork.CreateUser,
                Executor = phoneWork.Executor,
                Id = phoneWork.Id,
                IssueDesc = phoneWork.IssueDesc,
                Note= phoneWork.Note,
                ProgressDesc = phoneWork.ProgressDesc,
                Recorder = phoneWork.Recorder,
                Speaker = phoneWork.Speaker,
                Status = phoneWork.Status,
                TownId = phoneWork.TownId,
                UpdateDate = phoneWork.UpdateDate,
                UpdateUser = phoneWork.UpdateUser,
                PhoneTime = phoneWork.PhoneTime,
                DiasterId = phoneWork.DiasterId,
                
            };
        }

        public List<PhoneWorkModel> GetByDiasterId(int diasterId)
        {
            return PhoneWorkRepository.GetByDiasterId(diasterId);
        }

        public void Create(UserBriefModel user, PhoneWorkModel model)
        {
            model.CreateUser = user.UserName;
            model.UpdateUser = user.UserName;
            model.CreateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            PhoneWorkRepository.Create(model);
        }

        public AdminResultModel Delete(int id)
        {
            var entity = PhoneWorkRepository.Get(id);
            if (entity == null)
            {
                return new AdminResultModel
                {
                    ErrorMessage = "無此資料",
                    IsSuccess = false
                };
            }
            PhoneWorkRepository.Delete(id);
            return new AdminResultModel
            {
                IsSuccess = true,
                ErrorMessage = "成功"
            };
        }

        public AdminResultModel Update(UserBriefModel user, PhoneWorkViewModel model)
        {
            var entity = PhoneWorkRepository.Get(model.Id);
            if (entity == null)
            {
                return new AdminResultModel 
                {
                    ErrorMessage = "無此資料",
                    IsSuccess = false
                };
            }
            entity.PhoneTime = model.PhoneTime;
            entity.Executor = model.Executor;
            entity.IssueDesc = model.IssueDesc;
            entity.Note = model.Note;
            entity.ProgressDesc = model.ProgressDesc;
            entity.Recorder = model.Recorder;
            entity.Speaker = model.Speaker;
            entity.Status = model.Status;
            entity.TownId = model.TownId;
            entity.UpdateDate = DateTimeHelper.GetCurrentTime();
            entity.UpdateUser = user.UserName;
            PhoneWorkRepository.Update(entity);
            return new AdminResultModel 
            {
                IsSuccess = true,
                ErrorMessage = "修改成功"
            };
        }
    }
}