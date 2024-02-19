using MyEPA.Enums;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;

namespace MyEPA.Services
{
    public class FileDataService
    {
        FileRepository FileRepository = new FileRepository();
        public List<FileDataModel> GetBySource(SourceTypeEnum sourceType, int sourceId)
        {
            return FileRepository.GetBySource(sourceType, sourceId);
        }
        public FileDataModel Get(int id)
        {
            return FileRepository.Get(id);
        }
        public List<FileDataModel> GetListBySource(SourceTypeEnum sourceType)
        {
            return FileRepository.GetListBySource(sourceType);
        }
        public FileDataBaseModels GetFile(int id)
        {
            var fileData = FileRepository.Get(id);
            if(fileData == null)
            {
                return null;
            }
            var file = UploadFileHelper.GetFile(fileData.RealFileName);
            if (file != null) 
            {
                file.UserFileName = fileData.UserFileName;
                return file;
            }

            return default;
        }
        public AdminResultModel DeleteFile(int id)
        {
            var file = FileRepository.Get(id);
            if (file == null)
            {
                return new AdminResultModel 
                {
                    IsSuccess = false,
                    ErrorMessage = "資料不存在"
                };
            }
            
            UploadFileHelper.DeleteFile(file.RealFileName);

            FileRepository.Delete(id);

            return new AdminResultModel 
            { 
                IsSuccess = true
            };
        }
        public void DeleteFileBySource(SourceTypeEnum sourceType, int sourceId)
        {
            var files = FileRepository.GetBySource(sourceType, sourceId);
            if (files == null)
            {
                return;
            }
            foreach (var file in files)
            {
                UploadFileHelper.DeleteFile(file.RealFileName);

                FileRepository.Delete(file.Id);
            }
        }

        public int UploadFile(UploadFileBaseModel uploadFile, string fileName, bool isShowError = true)
        {
            FileUploadResultBaseModels uploadResult = uploadFile.File.UploadFile(fileName);

            if (uploadResult.IsSuccess == false)
            {
                //不顯示錯誤 上傳失敗回傳 - 1
                if (isShowError == false)
                {
                    return -1;
                }
                throw new Exception("檔案上傳失敗");
            }

            int id = FileRepository.CreateAndResultIdentity<int>(new FileDataModel
            {
                CreateDate = DateTimeHelper.GetCurrentTime(),
                CreateUser = uploadFile.User,
                RealFileName = uploadResult.FileData.RealFileNameAndExtension,
                SourceId = uploadFile.SourceId,
                SourceType = uploadFile.SourceType.ToInteger(),
                UserFileName = uploadResult.FileData.UserFileNameAndExtension,
                Description = uploadFile.Description
            });
            return id;
        }

        public int UploadFileByGuidName(UploadFileBaseModel uploadFile, bool isShowError = true)
        {
            return UploadFile(uploadFile, Guid.NewGuid().ToString(), isShowError);
        }
        /// <summary>
        /// 不開放指定名稱
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <param name="isShowError"></param>
        /// <returns></returns>
        private int UploadFile(UploadFileBaseModel uploadFile, bool isShowError = true)
        {
            return UploadFile(uploadFile, uploadFile.File.GetFileName(), isShowError);
        }
    }
}