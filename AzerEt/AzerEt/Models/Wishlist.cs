using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AzerEt.Models
{
    public class Wishlist
    {
        [Key]
        public int Id { get; set; }
        public int Count { get; set; } = 1;
        [ForeignKey("Menu")]
        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        [ForeignKey("CustomUser")]
        public string UserId { get; set; }
        public CustomUser CustomUser { get; set; }

    }
}
