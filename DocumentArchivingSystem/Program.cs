using Library.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentArchivingSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logic.Menu();

            using (var context = new DASDbContextcs())
            {
                context.Database.EnsureCreated();
            }

        }
    }
}
