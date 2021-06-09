using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Elearning.Api.Attributes;

namespace Elearning.Api.Controllers
{
    [ApiController]
    [Route("api/file")]
    public class FileController : BaseApiController
    {
        public FileController ()
        {

        }

        [HttpGet]
        [Route("{filename}")]
        [ResponseCache(Duration = 3600)]
        [AllowAnonymous]
        public async Task<IActionResult> GetFile ([FromRoute] string filename)
        {
            try
            {
                if (filename == null)
                    return Content("filename not present");

                var path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    filename);

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, GetContentType(path), Path.GetFileName(path));
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        private string GetContentType (string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes ()
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
                {".csv", "text/csv"}
            };
        }
    }
}
