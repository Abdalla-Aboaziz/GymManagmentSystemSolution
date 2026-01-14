using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentBLL.Service.Interfaces.AttachmentService
{
    public  interface IAttachmentService
    {

        string? Upload(string folderName, IFormFile file);

        bool Delete( string fileName , string folderName);
    }
}
