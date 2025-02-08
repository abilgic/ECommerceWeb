using ECommerceWebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWebApi.DataAccess
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<Entities.Account> Accounts { get; set; }
        public DbSet<Entities.Cart> Carts { get; set; }
        public DbSet<Entities.Category> Categories { get; set; }
        public DbSet<Entities.Payment> Payments { get; set; }
        public DbSet<Entities.Product> Products { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=localhost;Database=ECommerceDb;Trusted_Connection=True;");
        //    }
        //}

        
        


    }
}
