using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AzerEt.Models
{
    public class CheckWishlist
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Checkout")]
        public int CheckoutId { get; set; }
        public Checkout Checkout { get; set; }

        [ForeignKey("Menu")]
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        public int Count { get; set; }
    }
}
