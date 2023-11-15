using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AzerEt.Models
{
    public class Specials
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Category { get; set; }
       
        [Column(TypeName = "nText")]
        public string Title { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
