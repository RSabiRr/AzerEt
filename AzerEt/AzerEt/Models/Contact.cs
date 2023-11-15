using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzerEt.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100), Required]
        public string Name { get; set; }
        [MaxLength(100) , Required]
        public string Email { get; set; }
        [MaxLength(100), Required]
        public string Subject { get; set; }

        [Column(TypeName = "nText") , Required]
        public string Destiraction { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
