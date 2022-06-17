using FileManagement.Infrastructure.Responses;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileManagement.Core.Interfaces
{
    public interface IFileService
    {
        Task<List<FileResponse>> GetFilesAsync();

        Task<string> UploadFileAsync(IFormFile requestFile); 
    }
}
