using System;
using System.ComponentModel.DataAnnotations;

namespace FileManagement.Infrastructure.Models
{
    public class UploadedFile 
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }

        public string Size { get; set; }
        
        public DateTime UploadDate { get; set; } 
    }
}
