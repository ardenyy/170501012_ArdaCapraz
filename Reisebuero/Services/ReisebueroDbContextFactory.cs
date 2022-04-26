using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Reisebuero.Services
{
    public class ReisebueroDbContextFactory : IDesignTimeDbContextFactory<ReisebueroDbContext>
    {
        public ReisebueroDbContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<ReisebueroDbContext>();
            //options.UseSqlServer(ConfigurationManager.ConnectionStrings["ReisebueroDB"].ConnectionString);
            //options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ReisebueroDB;Integrated Security=True");
            options.UseSqlServer(Properties.Settings.Default.ConnectionString);
            return new ReisebueroDbContext(options.Options);
        }
    }
}
