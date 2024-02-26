using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyEPA.Helper
{
    public static class UploadFileHelper
    {
        private static string _DefaultFolder = "UserFiles";
        public static string GetFileExtension(this HttpPostedFileBase file)
        {
            return Path.GetExtension(file.FileName);
        }
        public static string GetFileName(this HttpPostedFileBase file)
        {
            return Path.GetFileNameWithoutExtension(file.FileName);
        }
        public static Stream GetFileStream(string fileName)
        {
            string filePath = GetServerMapPath(fileName);
            return GetFileToStream(filePath);
        }
        public static Stream GetFileToStream(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                Stream stream = new MemoryStream();

                fileStream.CopyTo(stream);

                stream.Seek(0, SeekOrigin.Begin);

                return stream;
            }
        }
        private static byte[] GetFileToByte(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                return stream.ToByte();
            }
        }

        public static string GetFileUrl(string fileName)
        {
            string filePath = GetServerMapPath(fileName);

            if (File.Exists(filePath) == false)
            {
                return null;
            }
            return filePath;
        }
        public static FileDataBaseModels GetFile(string fileName)
        {
            string filePath = GetServerMapPath(fileName);
            if (File.Exists(filePath) == false)
            {
                return null;
            }
            var file = GetFileToByte(filePath);
            
            return new FileDataBaseModels 
            {
                Extension = Path.GetExtension(fileName),
                FileByte = file,
                FileLength = file.Length,
                RealFileName = fileName,
                ServerMapPath = filePath,
            };
        }

        public static string GetServerMapPath(string folder,string fileName)
        {
            string path = $"S:/{folder}";
            if (SettingHelper.IsDeBug)
            {
                path = $"D:/temp_web/MyEPA/{folder}";
            }
            if (Directory.Exists(path) == false)
            {
                //新增資料夾
                Directory.CreateDirectory($"{path}");
            }

            return $"{path}/{fileName}";
        }
        public static string GetServerMapPath(string fileName)
        {
            return GetServerMapPath(_DefaultFolder, fileName);
        }
        public static void DeleteFile(string fileName)
        {
            string filePath = GetServerMapPath(fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        public static FileUploadResultBaseModels UploadFile(this HttpPostedFileBase file, string fileName)
        {
            if (file == null || file.ContentLength <= 0)
            {
                return new FileUploadResultBaseModels 
                {
                    Status = FileDataEnum.NoUploadFile
                };
            }
            string fileExtension = GetFileExtension(file);
            try
            {
                string path = Path.Combine(GetServerMapPath(_DefaultFolder, string.Empty), $"{fileName}{fileExtension}");
                int fileLength = file.ContentLength;
                file.SaveAs(path);
                return new FileUploadResultBaseModels
                {
                    FileData = new FileDataBaseModels
                    {
                        Extension = fileExtension,
                        FileByte = new BinaryReader(file.InputStream).ReadBytes(fileLength),
                        FileLength = fileLength,
                        UserFileName = GetFileName(file),
                        RealFileName = fileName
                    },
                    Status = FileDataEnum.Success
                };
            }
            catch
            {

            }
            return new FileUploadResultBaseModels 
            {
                Status = FileDataEnum.Failure
            };
        }

        public static void DeleteFileByPath(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public static bool IsExistsByFileName(string fileName)
        {
            string filePath = GetServerMapPath(fileName);
            return IsExists(filePath);
        }
        public static bool IsExists(string path)
        {
            return File.Exists(path);
        }
    }
}