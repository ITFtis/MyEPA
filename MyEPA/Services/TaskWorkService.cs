using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Web;

namespace MyEPA.Services
{

    public class TaskWorkService
    {
        TaskWorkRepository TaskWorkRepository = new TaskWorkRepository();
        public List<TaskWorkModel> GetAll()
        {
            return TaskWorkRepository.GetList();
        }

        public TaskWorkViewModel Get(int id)
        {
            var TaskWork = TaskWorkRepository.Get(id);
            return new TaskWorkViewModel 
            {
                CreateDate = TaskWork.CreateDate,
                CreateUser = TaskWork.CreateUser,
                Executor = TaskWork.Executor,
                Id = TaskWork.Id,
                IssueDesc = TaskWork.IssueDesc,
                Note= TaskWork.Note,
                ProgressDesc = TaskWork.ProgressDesc,
                Status = TaskWork.Status,
                TownId = TaskWork.TownId,
                UpdateDate = TaskWork.UpdateDate,
                UpdateUser = TaskWork.UpdateUser,
                PhoneTime = TaskWork.PhoneTime,
            };
        }

        internal void Create(UserBriefModel user, TaskWorkModel model)
        {
            model.CreateUser = user.UserName;
            model.UpdateUser = user.UserName;
            model.CreateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            TaskWorkRepository.Create(model);
        }

        public AdminResultModel Delete(int id)
        {
            var entity = TaskWorkRepository.Get(id);
            if (entity == null)
            {
                return new AdminResultModel
                {
                    ErrorMessage = "無此資料",
                    IsSuccess = false
                };
            }
            TaskWorkRepository.Delete(id);
            return new AdminResultModel
            {
                IsSuccess = true,
                ErrorMessage = "成功"
            };
        }

        public AdminResultModel Update(UserBriefModel user, TaskWorkViewModel model)
        {
            var entity = TaskWorkRepository.Get(model.Id);
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
            entity.Status = model.Status;
            entity.TownId = model.TownId;
            entity.UpdateDate = DateTimeHelper.GetCurrentTime();
            entity.UpdateUser = user.UserName;
            TaskWorkRepository.Update(entity);
            return new AdminResultModel 
            {
                IsSuccess = true,
                ErrorMessage = "修改成功"
            };
        }
    }
}