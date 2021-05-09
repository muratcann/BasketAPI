using Basket.Model.Dto;
using Microsoft.EntityFrameworkCore;

namespace Basket.Model.EntityFramework
{
    public class EFDbContext : DbContext
    {
        public EFDbContext(DbContextOptions<EFDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
