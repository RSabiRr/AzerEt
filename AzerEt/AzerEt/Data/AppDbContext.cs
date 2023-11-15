using AzerEt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace AzerEt.Data
{
    public class AppDbContext : IdentityDbContext
    {

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Whyus> Whyus { get; set; }
        public DbSet<CustomUser> CustomUsers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<CheckWishlist> CheckWishlists { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Specials> Specials { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Cheif> Cheifs { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }


    }
}
