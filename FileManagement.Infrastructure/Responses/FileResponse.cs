using FileManagement.Infrastructure.Models;
using System.Collections.Generic;

namespace FileManagement.Infrastructure.Responses
{
    public class FileResponse
    {
        public string ContentType { get; set; }

        public List<UploadedFile> Files { get; set; } 
    }
}
