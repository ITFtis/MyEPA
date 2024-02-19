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
    public class ContactManualSupervisionFileDataService
    {
        FileRepository FileRepository = new FileRepository();
        FileDataService FileDataService = new FileDataService();
        SourceTypeEnum sourceType = SourceTypeEnum.Supervision;
        public List<ContactManualSupervisionFileDataViewModel> GetList()
        {
            var files = FileRepository.GetListBySource(sourceType);

            return files.ConvertToModel<FileDataModel, ContactManualSupervisionFileDataViewModel>((input, output) =>
            {
                output.Chapter = ((SupervisionSourceEnum)input.SourceId).GetDescription();
            }).ToList();
        }
        public ContactManualSupervisionFileDataUploadViewModel GetBySourceId(int sourceId)
        {
            var fileData = FileRepository.GetBySource(sourceType, sourceId).SingleOrDefault();

            return new ContactManualSupervisionFileDataUploadViewModel
            {
                Id = fileData.Id,
                SourceId = fileData.SourceId,
                Description = fileData.Description,
                SourceType = sourceType,
                UserFileName = fileData.UserFileName,
                RealFileName = fileData.RealFileName
            };
        }
        public int UploadFile(UploadFileBaseModel uploadFile)
        {
            //存在就先刪除
            if (IsExists(uploadFile.SourceId))
            {
                FileDataService.DeleteFileBySource(sourceType, uploadFile.SourceId);
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
            return FileRepository.IsExistsByTypeAndSourceId(sourceType, sourceId);
        }
    }
}