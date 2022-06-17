using FileManagement.Business.Constant;
using FileManagement.Business.Extensions;
using FileManagement.Core.Interfaces;
using FileManagement.Infrastructure.Data;
using FileManagement.Infrastructure.Models;
using FileManagement.Infrastructure.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagement.Business.Services
{
    public class FileService : IFileService
    {
        // Inject file DB context
        private readonly FileContext _context;
        private readonly int FileSizeLimit = 5000000;

        public FileService(FileContext context)
        {
            _context = context;
        }

        public async Task<List<FileResponse>> GetFilesAsync()
        {
            try
            {
                // Execute context task to return file records in groups of content type
                var files = (await _context.Files.ToListAsync()).GroupBy(t => t.ContentType,
                    // Map key-value to content type and file metadata
                    (k, v) => new FileResponse
                    {
                        ContentType = k,
                        Files = v.ToList()
                    }).ToList();

                // Validate empty data sets
                if (files == null)
                {
                    return new List<FileResponse>();
                }

                // Return grouped file records
                return files;
            }
            catch
            {
                throw;
            }
            finally
            {
                // Dispose context and free resources after every call
                await _context.DisposeAsync();
            }
        }

        public async Task<string> UploadFileAsync(IFormFile requestFile)
        {
            try
            {
                // Store file details into variables
                var fileName = requestFile.FileName;
                var fileSize = requestFile.Length;
                var fileExtension = Path.GetExtension(fileName);

                // Validate state of request form
                if (requestFile == null || string.IsNullOrWhiteSpace(fileName) || fileSize == 0)
                {
                    return ResponseStatuses.InvalidRequestBody;
                }

                // Validate file extension for certain file types only
                if (!fileExtension.ToLower().IsValidFileExtension())
                {
                    return ResponseStatuses.InvalidFileType;
                }

                // Return the current list of file records
                var dbFiles = await _context.Files.ToListAsync();

                // Validate duplicated file name against database records
                if (dbFiles.Any(t => t.Name.Equals(fileName)))
                {
                    return ResponseStatuses.FileExists;
                }

                // Validate file size for a certain limit
                if (fileSize <= 0 || fileSize > FileSizeLimit)
                {
                    return ResponseStatuses.InvalidFileSize;
                }

                // Instantiante a new file object and build its properties with API request data
                var uploadedFile = new UploadedFile()
                {
                    Name = fileName,
                    Size = $"{fileSize} bytes",
                    ContentType = requestFile.ContentType,
                    UploadDate = DateTime.UtcNow
                };

                // Add file to the database 
                await _context.Files.AddAsync(uploadedFile);
                // For consideration of no limit to file count and requests
                await _context.SaveChangesAsync();

                return ResponseStatuses.ValidFile;
            }
            catch
            {
                return ResponseStatuses.InternalServerError;
            }
            finally
            {
                // Dispose context and free resources after every call
                await _context.DisposeAsync();
            }
        }
    }
}
