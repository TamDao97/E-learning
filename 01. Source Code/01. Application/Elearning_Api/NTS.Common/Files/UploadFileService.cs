using Elearning.Model.Models.File;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using NTS.Common;
using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Files
{
    public class UploadFileService : IUploadFileService
    {
        private IWebHostEnvironment _hostingEnvironment;
        private readonly UploadSettingModel uploadSettingModel;
        public UploadFileService(IWebHostEnvironment environment, IOptions<UploadSettingModel> option)
        {
            _hostingEnvironment = environment;
            this.uploadSettingModel = option.Value;
        }

        /// <summary>
        /// Upload 1 file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folderName">Thư mục lưu trữ</param>
        /// <returns></returns>
        public async Task<UploadResultModel> UploadFile(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0038);
            }

            // Tạo tên file lưu trức
            var extension = Path.GetExtension(file.FileName);
            string fileName = Guid.NewGuid().ToString() + extension;

            if (!uploadSettingModel.Extensions.Contains(extension.ToLower()))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0039, extension);
            }

            string pathFolder = Path.Combine(uploadSettingModel.FolderUpload, folderName);
            string pathFolderServer = Path.Combine(Directory.GetCurrentDirectory(), pathFolder);

            // Kiểm tra folder upload
            if (!Directory.Exists(pathFolderServer))
            {
                Directory.CreateDirectory(pathFolderServer);
            }

            string pathFile = Path.Combine(pathFolderServer, fileName);

            // Lưu file
            using (var stream = new FileStream(pathFile, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            UploadResultModel fileResult = new UploadResultModel();
            fileResult.FileSize = file.Length;
            fileResult.FileUrl = Path.Combine(pathFolder, fileName).Replace("\\", "/");
            fileResult.FileName = file.FileName;

            return fileResult;
        }

        public async Task<List<UploadResultModel>> UploadFiles(List<IFormFile> files, string folderName)
        {
            if (files == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0038);
            }

            List<UploadResultModel> results = new List<UploadResultModel>();

            UploadResultModel result;
            foreach (var file in files)
            {
                result = await UploadFile(file, folderName);

                results.Add(result);
            }

            return results;
        }

        public async Task<byte[]> DownloadFiles(DownloadFileModel files)
        {
            List<ArchiveFileModel> archiveFiles = new List<ArchiveFileModel>();

            foreach (var item in files.Files)
            {
                ArchiveFileModel file = new ArchiveFileModel();
                file.Name = Path.GetFileNameWithoutExtension(item.PathFile);
                file.Extension = Path.GetExtension(item.PathFile);
                string pathFolder = Path.Combine(item.PathFile);
                string pathFolderServer = Path.Combine(Directory.GetCurrentDirectory(), pathFolder);
                file.FileBytes = File.ReadAllBytes(pathFolderServer);

                archiveFiles.Add(file);

            }
            return await ZipFiles(archiveFiles);
        }

        public async Task<byte[]> ZipFiles(List<ArchiveFileModel> files)
        {
            using (var packageStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(packageStream, ZipArchiveMode.Create, true))
                {
                    foreach (var virtualFile in files)
                    {
                        //Create a zip entry for each attachment
                        var zipFile = archive.CreateEntry(virtualFile.Name + virtualFile.Extension);

                        using (MemoryStream originalFileMemoryStream = new MemoryStream(virtualFile.FileBytes))
                        {
                            using (var zipEntryStream = zipFile.Open())
                            {
                                await originalFileMemoryStream.CopyToAsync(zipEntryStream);
                            }
                        }
                    }
                }

                return packageStream.ToArray();
            }

        }

        public async Task DeleteFile(DeleteFileModel model)
        {
            if (!string.IsNullOrEmpty(model.Avatar))
            {
                //var fileName = Directory.GetFiles(model.Avatar);
                var rootFolder = Directory.GetCurrentDirectory();
                string pathFolderServer = Path.Combine(Directory.GetCurrentDirectory(), rootFolder);
                //string path = File.Exists(Path.Combine(model.Avatar));
                var path = rootFolder + "/" + model.Avatar;
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                //else
                //{
                //    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.File);
                //}    
            }

        }

        /// <summary>
        /// Download 1 file
        /// </summary>
        /// <param name="fileModel"></param>
        /// <returns></returns>
        public async Task<FileResultModel> DownloadFile(FileModel fileModel)
        {
            if (string.IsNullOrEmpty(fileModel.PathFile))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, "File");
            }

            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                fileModel.PathFile);

            if (!File.Exists(path))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, "File");
            }

            FileResultModel fileResultModel = new FileResultModel();
            using (var memory = new MemoryStream())
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                fileResultModel.FileStream = memory.ToArray();
            }

            fileResultModel.ContentType = GetContentType(fileModel.PathFile);
            fileResultModel.FileName = GetFileName(fileModel.PathFile, fileModel.NameFile);

            return fileResultModel;
        }

        public string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        /// <summary>
        /// Lấy tên file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFileName(string path, string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string extensionInPath = Path.GetExtension(path);
            string fileNameNew = string.Empty;
            if (string.IsNullOrEmpty(extension))
            {
                fileNameNew = fileName + extensionInPath;
            }
            else if (extension.Equals(extensionInPath))
            {
                fileNameNew = fileName.Remove(fileName.LastIndexOf(".")) + extensionInPath;
            }

            return fileNameNew;
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".zip","application/zip" }
            };
        }

        public async Task<UploadResultModel> UploadFileTemplate(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0038);
            }

            // Tạo tên file lưu trức
            var extension = Path.GetExtension(file.FileName);

            if (!uploadSettingModel.Extensions.Contains(extension.ToLower()))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0039, extension);
            }

            string pathFolder = Path.Combine(uploadSettingModel.FolderUpload, folderName);
            string pathFolderServer = Path.Combine(Directory.GetCurrentDirectory(), pathFolder);

            // Kiểm tra folder upload
            if (!Directory.Exists(pathFolderServer))
            {
                Directory.CreateDirectory(pathFolderServer);
            }

            string pathFile = Path.Combine(pathFolderServer, file.FileName);

            // Lưu file
            using (var stream = new FileStream(pathFile, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            UploadResultModel fileResult = new UploadResultModel();
            fileResult.FileSize = file.Length;
            fileResult.FileUrl = Path.Combine(pathFolder, file.FileName).Replace("\\", "/");
            fileResult.FileName = file.FileName;

            return fileResult;
        }

        /// <summary>
        /// Upload 1 file zip
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folderName">Thư mục lưu trữ</param>
        /// <returns></returns>
        public async Task<UploadResultModel> UploadFileZip(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0038);
            }

            // Tạo tên file lưu trức
            var extension = Path.GetExtension(file.FileName);
            string fileName = Guid.NewGuid().ToString() + extension;

            if (!Constants.File_Upload_Zip.ToLower().Equals(extension.ToLower()))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0052, extension);
            }

            string pathFolder = Path.Combine(uploadSettingModel.FolderUpload, folderName);
            string pathFolderServer = Path.Combine(Directory.GetCurrentDirectory(), pathFolder);
            string pathFolderZipServer = Path.Combine(Directory.GetCurrentDirectory(), pathFolder + "/" + Path.GetFileNameWithoutExtension(fileName));

            // Kiểm tra folder upload
            if (!Directory.Exists(pathFolderServer))
            {
                Directory.CreateDirectory(pathFolderServer);
            }

            if (!Directory.Exists(pathFolderZipServer))
            {
                Directory.CreateDirectory(pathFolderZipServer);
            }
            else
            {
                Directory.Delete(pathFolderZipServer, true);
                Directory.CreateDirectory(pathFolderZipServer);
            }

            string pathFile = Path.Combine(pathFolderServer, fileName);

            if (File.Exists(pathFile))
            {
                File.Delete(pathFile);
            }

            // Lưu file
            using (var stream = new FileStream(pathFile, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            ZipFile.ExtractToDirectory(pathFile, pathFolderZipServer);

            //File.Delete(pathFile);

            UploadResultModel fileResult = new UploadResultModel();
            fileResult.FileSize = file.Length;
            fileResult.FileUrl = Path.Combine(pathFolder, Path.GetFileNameWithoutExtension(fileName) + "/res/index.html").Replace("\\", "/");
            fileResult.FileName = file.FileName;

            return fileResult;
        }
    }
}
