using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AzerEt.Models
{
    public class About
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nText")]
        public string Title { get; set; }
    }
}
