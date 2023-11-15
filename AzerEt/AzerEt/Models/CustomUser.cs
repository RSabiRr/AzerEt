using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzerEt.Models
{
    public class CustomUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public int Count { get; set; }
        public bool IsAdmin { get; set; }

        [NotMapped, Required]
        public string RoleId { get; set; }

        public List<Wishlist> Wishlists { get; set; }
        public List<Checkout> Checkouts { get; set; }
        //public List<CheckToWish> CheckToWishes { get; set; }


        //public List<Wishlist> Wishlists { get; set; }

    }
}
