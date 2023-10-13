using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using SimpleCRUDwebAPI.Models;

namespace SimpleCRUDwebAPI.DAL
{
    public class MyAppDbContext : DbContext
    {

        private readonly IConfiguration configuration;

        public MyAppDbContext(IConfiguration config)
        {
            configuration = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
       
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Users> Users { get; set; }
       
    }
}
