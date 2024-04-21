using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public class DASDbContext : DbContext
    {
        public DbSet<DocumentArchivingSystem.Document> Documents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=DASdatabase.db");
        }
    }
}
