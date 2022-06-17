namespace FileManagement.Business.Extensions 
{
    public static class FileExtensionValidator
    {
        // Extension method to validate file extension for accepted types
        public static bool IsValidFileExtension(this string ext)
        {
            return ext == ".doc" 
                || ext == ".docx" 
                || ext == ".xls" 
                || ext == ".xlsx" 
                || ext == ".pdf"
                || ext == ".txt" 
                || ext == ".json" 
                || ext == ".png" 
                || ext == ".jpg" 
                || ext == ".gif" 
                || ext == ".mp4" 
                || ext == ".mov" 
                || ext == ".zip";
        }
    }
}
