using GymManagmentBLL.Service.Interfaces.AttachmentService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Classes.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        // Allowed file extensions for upload (security & validation purpose)
        private readonly string[] _allowedextention =  { ".png", ".jpg", ".jpeg" };

        // Maximum allowed file size (5 MB)
        private readonly long _maxAllowedSize = 5*1024*1024;

        // Used to access wwwroot path
        private readonly IWebHostEnvironment _webHost;

        public AttachmentService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }
        public string? Upload(string folderName, IFormFile file)
        {
            try
            {
                // Basic validation
                if (folderName is null || file is null || file.Length == 0) return null;

                // Validate file size
                if (file.Length > _maxAllowedSize) return null;

                // Validate file extension
                var fileExtention = Path.GetExtension(file.FileName).ToLower();
                if (!_allowedextention.Contains(fileExtention)) return null;

                // Build folder path: wwwroot/images/{folderName}
                var folderpath = Path.Combine(_webHost.WebRootPath, "images", folderName);

                // Ensure directory exists
                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }
                // Generate unique file name to avoid overwriting
                var fileName = Guid.NewGuid().ToString() + fileExtention;
                var filePath = Path.Combine(folderpath, fileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);
                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to Upload file in Folder={folderName}:{ex} ");
                return null;
            }


        }
        public bool Delete(string fileName, string folderName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return false;
                // Build full file path
                var fullPath = Path.Combine(_webHost.WebRootPath, "images", folderName, fileName);
                // Delete file if it exists
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to Upload file in Folder={folderName}:{ex} ");
                return false;
            }
        }

      
    }
}
