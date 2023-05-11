using ExcelToSqlProgram.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelToSqlProgram.Data
{
    public class ApplicationDbContext : DbContext
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        public DbSet<ExcelData> ExcelData { get; set; }
    }
}
