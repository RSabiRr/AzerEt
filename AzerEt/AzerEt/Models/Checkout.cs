using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzerEt.Models
{
    public class Checkout
    {
        [Key]
        public int Id { get; set; }
        public string Adress { get; set; }
        public string Information { get; set; }
        public string ContactPhone { get; set; }
        public bool Iscart { get; set; } = false;
        public bool isTrue { get; set; } = false;

        public int TotalPrice { get; set; }
        [ForeignKey("CustomUser")]
        public string UserId { get; set; }
        public CustomUser CustomUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<CheckWishlist> CheckWishlists { get; set; }
    }

}
