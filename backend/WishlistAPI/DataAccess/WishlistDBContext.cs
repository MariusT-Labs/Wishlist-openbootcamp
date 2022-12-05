using Microsoft.EntityFrameworkCore;
using WishlistAPI.Models;

namespace WishlistAPI.DataAccess
{
    public class WishlistDBContext: DbContext
    {
        public WishlistDBContext(DbContextOptions<WishlistDBContext> options): base(options) { }

        public DbSet<Product>? Products { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Rating>? Ratings { get; set; }
    }
}
