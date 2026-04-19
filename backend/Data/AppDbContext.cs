using Microsoft.EntityFrameworkCore;
using Order.API.Models;

namespace Order.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Order.API.Models.Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Bill> Bills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Order configuration
            modelBuilder.Entity<Order.API.Models.Order>()
                .HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<Order.API.Models.Order>()
                .HasOne(o => o.Bill)
                .WithOne()
                .HasForeignKey<Bill>(b => b.OrderId);

            // OrderItem configuration
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.MenuItem)
                .WithMany()
                .HasForeignKey(oi => oi.MenuItemId);

            // Seed initial menu items
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem { Id = 1, Name = "Bia Hà Nội", Description = "Bia Hà Nội 330ml", Price = 15000, Category = "Đồ uống", IsAvailable = true },
                new MenuItem { Id = 2, Name = "Bia Saigon", Description = "Bia Saigon 330ml", Price = 15000, Category = "Đồ uống", IsAvailable = true },
                new MenuItem { Id = 3, Name = "Gỏi cuốn", Description = "Gỏi cuốn tôm thịt", Price = 30000, Category = "Khai vị", IsAvailable = true },
                new MenuItem { Id = 4, Name = "Đậu phụ chiên", Description = "Đậu phụ chiên vàng", Price = 25000, Category = "Khai vị", IsAvailable = true },
                new MenuItem { Id = 5, Name = "Gà nướng", Description = "Gà nướng ở Lò", Price = 120000, Category = "Chính", IsAvailable = true },
                new MenuItem { Id = 6, Name = "Cá chiên", Description = "Cá chiên giòn", Price = 100000, Category = "Chính", IsAvailable = true },
                new MenuItem { Id = 7, Name = "Thịt dê nướng", Description = "Thịt dê nướng thơm", Price = 150000, Category = "Chính", IsAvailable = true },
                new MenuItem { Id = 8, Name = "Nước chanh", Description = "Nước chanh tươi", Price = 10000, Category = "Đồ uống", IsAvailable = true }
            );
        }
    }
}