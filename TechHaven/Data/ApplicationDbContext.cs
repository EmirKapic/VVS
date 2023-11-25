using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechHaven.Models;

namespace TechHaven.Data
{
    public class ApplicationDbContext : IdentityDbContext<Customer>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrator> Administrator { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrator>().ToTable("Administrator");
            modelBuilder.Entity<Customer>().ToTable("Customer")
                .HasMany(c => c.Products)
                .WithMany(p => p.Customers);
            modelBuilder.Entity<Order>().ToTable("Order")
                .HasMany(o => o.Products)
                .WithMany(p => p.Orders);
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<ShoppingCart>().ToTable("ShoppingCart")
                .HasMany(s => s.Products)
                .WithMany(p => p.ShoppingCarts);
            modelBuilder.Entity<Image>().ToTable("Image");
            modelBuilder.Entity<PaymentMethod>().ToTable("PaymentMethod");
            base.OnModelCreating(modelBuilder);
        }
    }
}