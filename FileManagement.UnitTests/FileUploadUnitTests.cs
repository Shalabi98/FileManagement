using FileManagement.Business.Constant;
using FileManagement.Business.Services;
using FileManagement.Core.Interfaces;
using FileManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace FileManagement.UnitTests
{
    public class FileUploadUnitTests
    {
        private readonly IFileService _fileService;

        public FileUploadUnitTests()
        {
            _fileService = new FileService(new FileContext());
        }

        [Fact]
        public void FileUpload_Returns_InvalidRequestBody_When_FormFile_Data_Is_Null() 
        {
            FormFile file = new FormFile(null, 0, 0, null, null);

            var responseStatus = _fileService.UploadFileAsync(file).Result;

            Assert.Equal(ResponseStatuses.InvalidRequestBody, responseStatus);
        }

        [Fact]
        public void FileUpload_Returns_InvalidFileSize_When_FileSize_Exceeds_SizeLimit()
        {
            FormFile file = new FormFile(null, 0, 5000010, "test.png", "test.png")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };

            var responseStatus = _fileService.UploadFileAsync(file).Result;

            Assert.Equal(ResponseStatuses.InvalidFileSize, responseStatus);
        }

        [Fact]
        public void FileUpload_Returns_ValidFile_When_FileSize_Within_SizeLimit_Or_FileExists_When_SameFile_Exists() 
        {
            FormFile file = new FormFile(null, 0, 1000, "test.pdf", "test.pdf")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            var responseStatus = _fileService.UploadFileAsync(file).Result;

            // Two possible assertions need to be made 
            if (responseStatus == ResponseStatuses.FileExists)
            {
                Assert.Equal(ResponseStatuses.FileExists, responseStatus);
            } 
            else
            {
                Assert.Equal(ResponseStatuses.ValidFile, responseStatus); 
            }
        }

        [Fact]
        public void FileUpload_Returns_InvalidFileType_When_FileType_Not_Supported()
        {
            FormFile file = new FormFile(null, 0, 20000, "test.bin", "test.bin")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream"
            };

            var responseStatus = _fileService.UploadFileAsync(file).Result;

            Assert.Equal(ResponseStatuses.InvalidFileType, responseStatus);
        }

        [Fact]
        public void FileUpload_Returns_ValidFile_When_FileType_Is_Supported_Or_FileExists_When_SameFile_Exists()
        {
            FormFile file = new FormFile(null, 0, 1000, "test.json", "test.json")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/json"
            };

            var responseStatus = _fileService.UploadFileAsync(file).Result;

            // Two possible assertions need to be made
            if (responseStatus == ResponseStatuses.FileExists)
            {
                Assert.Equal(ResponseStatuses.FileExists, responseStatus);
            }
            else
            {
                Assert.Equal(ResponseStatuses.ValidFile, responseStatus);
            }
        }
    }
}
