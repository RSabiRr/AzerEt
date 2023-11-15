using AzerEt.Models;
using System.Reflection.Metadata;

namespace AzerEt.ViewModels
{
    public class VmHome
    {
        public Contact Contacts { get; set; }

        public  List<Contact> Contactss { get; set; }

        public List<Menu> Menus { get; set; }
        public About Abouts { get; set; }

        public List<Cheif> Cheifs { get; set; }

        public List<Specials> Specials { get; set; }
        public List<Gallery> Galleries { get; set; }
        public List<Whyus> Whyus { get; set; }
        public List<Wishlist> Wishlists { get; set; }
        public List<CheckWishlist> CheckWishlists { get; set; }
        public Wishlist Wishlistss { get; set; }
        public Checkout Checkout { get; set; }
        public List<Checkout> Checkouts { get; set; }
        public List<Category> Categories { get; set; }
        public Setting Setting { get; set; }
        public CustomUser CustomUser { get; set; }
        public List<CustomUser>CustomUserss { get; set; }

        public int Total { get; set; }
        public int PageCount { get; set; }
        public double ItemCount { get; set; }
        public int Page { get; set; }



    }
}
