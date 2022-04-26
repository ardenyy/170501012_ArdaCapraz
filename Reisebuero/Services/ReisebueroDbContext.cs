using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Reisebuero.Models;

namespace Reisebuero.Services
{
    public class ReisebueroDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<LoginForm> LoginForms { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<TourSale> TourSales { get; set; }

        public ReisebueroDbContext(DbContextOptions options) : base(options) { }
    }
}
