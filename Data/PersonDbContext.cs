using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebApiPractice.Models;

namespace WebApiPractice.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define your DbSets (tables) here
        public DbSet<Person> Person { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderProduct> OrderProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(o=>o.Products)
                .WithMany(o=>o.Orders)
                .UsingEntity<OrderProduct>();

        }
    }
    
}
