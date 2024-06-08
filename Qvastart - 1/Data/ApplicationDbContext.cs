using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Qvastart___1.Models;

namespace Qvastart___1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }
        
        public DbSet<QvaUser> QvaUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<UserWishlistedProducts> UserWishlistsProductsTB { get; set; }
        public DbSet<UserPurchasedProducts> UserPurchasedProductsTB { get; set; }
        public DbSet<Image> ImagesTB { get; set; }

    }
}
