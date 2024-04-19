using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public class DASDbContextcs : DbContext
    {
        public static DbSet<DocumentArchivingSystem.Document> Documents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=myDb;Trusted_Connection=True;");
        }
    }
}
