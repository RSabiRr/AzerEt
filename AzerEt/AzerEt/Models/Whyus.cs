using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AzerEt.Models
{
    public class Whyus
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Uptitle { get; set; }
        [Column(TypeName = "nText")]
        public string Title { get; set; }
    }
}
