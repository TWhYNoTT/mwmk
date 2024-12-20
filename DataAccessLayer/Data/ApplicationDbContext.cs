using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Data
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options) 
        {
            
        }
        public DbSet<Marketer> Marketers { get; set; }
        public DbSet<UserMessage> UserMessages{ get; set; }
        public DbSet<favourite_product> favourite_products { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<IncomePeriod> IncomePeriods { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set;}
        public DbSet<ShopingCart> shopingCarts { get; set;}        
        public DbSet<OrderHeader> OrderHeaders { get; set;}
        public DbSet<OrderDetail> OrderDetails { get; set;}
        public DbSet<Review> Reviews { get; set;}
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<OrderDetail>()
        //        .HasKey(od => od.OrderId); // This should be correctly set up

        //    modelBuilder.Entity<OrderDetail>()
        //        .Property(od => od.OrderId)
        //        .ValueGeneratedOnAdd(); // Ensure it's marked as an identity column
        //    modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        //    {
        //        entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
        //        // Other configurations if needed
        //    });
        //}
    }
}
