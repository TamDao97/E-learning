using Elearning.Model.Models.File;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Files
{
    public interface IUploadFileService
    {
        public Task<UploadResultModel> UploadFile(IFormFile file, string folderName);
        Task<List<UploadResultModel>> UploadFiles(List<IFormFile> files, string folderName);
        Task<byte[]> DownloadFiles(DownloadFileModel files);
        Task<byte[]> ZipFiles(List<ArchiveFileModel> files);
        public Task DeleteFile(DeleteFileModel model);
        Task<FileResultModel> DownloadFile(FileModel fileModel);
        Task<UploadResultModel> UploadFileTemplate(IFormFile file, string folderName);
        Task<UploadResultModel> UploadFileZip(IFormFile file, string folderName);
    }
}
