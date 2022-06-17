namespace FileManagement.Business.Constant
{
    public static class ResponseStatuses
    {
		public const string InvalidRequestBody = "INVALID_REQUEST_BODY";
		public const string InvalidFileType = "INVALID_FILE_TYPE";
		public const string FileExists = "FILE_EXISTS";
		public const string InvalidFileSize = "INVALID_FILE_SIZE";
		public const string ValidFile = "VALID_FILE";
		public const string InternalServerError = "UNKNOWN_ERROR";
	}
}
