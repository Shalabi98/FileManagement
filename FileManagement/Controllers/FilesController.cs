using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileManagement.Core.Interfaces;
using FileManagement.Business.Constant;

namespace FileManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        // Inject file service
        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFilesAsync()
        {
            // Call file service to return list of file records
            var files = await _fileService.GetFilesAsync();

            // Validate file records 
            if (files == null || files.Count == 0)
            {
                return NotFound(new
                {
                    Status = "NO_FILES_FOUND",
                    Message = "No file records currently exist in the database."
                });
            }

            return Ok(files);
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> PostFilesAsync()
        {
            // Call file service to validate file data 
            var fileStatus = await _fileService.UploadFileAsync(Request.Form.Files[0]);

            // Return status and message based on error response statuses returned from the service
            switch (fileStatus)
            {
                case ResponseStatuses.InvalidRequestBody:
                    return BadRequest(new
                    {
                        Status = fileStatus,
                        Message = "No form files detected in the API request body."
                    });
                case ResponseStatuses.InvalidFileType:
                    return BadRequest(new
                    {
                        Status = fileStatus,
                        Message = "Only the following file extensions are supported: " +
                        "(.doc, .docx, .xls, .xlsx, .pdf, .txt, .json, .png, .jpg, .gif, .mp4, .mov, .zip)"
                    });
                case ResponseStatuses.FileExists:
                    return BadRequest(new
                    {
                        Status = fileStatus,
                        Message = "Duplicated file detected. Try again."
                    });
                case ResponseStatuses.InvalidFileSize:
                    return BadRequest(new
                    {
                        Status = fileStatus,
                        Message = "The maximum file size allowed is 5MB."
                    });
                case ResponseStatuses.InternalServerError:
                    return StatusCode(500, new
                    {
                        Status = fileStatus,
                        Message = "Internal server error."
                    });
            }

            return Ok(new { Status = "FILE_UPLOADED", Message = "File Uploaded Successfully." });
        }
    }
}
