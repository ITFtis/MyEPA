using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class ContactManualFileDataService
    {
        FileRepository FileRepository = new FileRepository();
        FileDataService FileDataService = new FileDataService();
        ContactManualDepartmentRepository ContactManualDepartmentRepository = new ContactManualDepartmentRepository();
        public List<ContactManualFileDataViewModel> GetListBySource(SourceTypeEnum sourceType)
        {
            var files = FileRepository.GetListBySource(sourceType);
            var departments = ContactManualDepartmentRepository.GetListByFilter(new ContactManualDepartmentParameter
            {
                Ids = files.Select(e => e.SourceId)
            }).ToDictionary(e => e.Id, e => e.Name);
            return files.ConvertToModel<FileDataModel, ContactManualFileDataViewModel>((input, output) =>
            {
                output.DepartmentName = departments.GetValue(input.SourceId);
            }).ToList();
        }
        public ContactManualFileDataUploadViewModel GetBySource(SourceTypeEnum sourceType, int sourceId)
        {
            var fileData = FileRepository.GetBySource(sourceType, sourceId).SingleOrDefault();
            
            if(fileData == null)
            {
                return null;
            }
            return new ContactManualFileDataUploadViewModel 
            {
                Id = fileData.Id,
                DepartmentId = fileData.SourceId,
                Description = fileData.Description,
                SourceType = (SourceTypeEnum)fileData.SourceType,
                UserFileName = fileData.UserFileName,
                RealFileName = fileData.RealFileName
            };
        }
        public int UploadFile(UploadFileBaseModel uploadFile)
        {
            //存在就先刪除
            if (IsExists(uploadFile.SourceId))
            {
                FileDataService.DeleteFileBySource(uploadFile.SourceType, uploadFile.SourceId);
            }
            return FileDataService.UploadFileByGuidName(uploadFile, false);
        }

        public FileDataBaseModels GetFile(int id)
        {
            var fileData = FileRepository.Get(id);
            if (fileData == null)
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
        private bool IsExists(int sourceId)
        {
            return ContactManualDepartmentRepository.IsExistsByFilter(new ContactManualDepartmentParameter
            {
                SourceIds = sourceId.ToListCollection(),
                
            });
        }
    }
}