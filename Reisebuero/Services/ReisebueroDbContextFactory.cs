using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Reisebuero.Services
{
    public class ReisebueroDbContextFactory : IDesignTimeDbContextFactory<ReisebueroDbContext>
    {
        public ReisebueroDbContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<ReisebueroDbContext>();
            options.UseSqlServer(Properties.Settings.Default.ConnectionString);
            return new ReisebueroDbContext(options.Options);
        }
    }
}
