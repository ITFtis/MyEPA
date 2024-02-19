using MyEPA.Enums;

namespace MyEPA.Models
{
    public class FileUploadResultBaseModels
    {
        public FileDataBaseModels FileData { get; set; }
        public FileDataEnum Status { get; set; }
        public bool IsSuccess 
        { 
            get 
            { 
                return Status == FileDataEnum.Success; 
            } 
        }
    }
}