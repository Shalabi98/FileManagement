using FileManagement.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace FileManagement.Infrastructure.Data
{
    public class FileContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Local DB Server using Windows Authentication
            optionsBuilder.UseSqlServer("Server=.;Database=FileManagement;Integrated Security=true;Trusted_Connection=true;");
        }

        public DbSet<UploadedFile> Files { get; set; }
    }
}
