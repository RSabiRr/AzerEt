using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AzerEt.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nText")]
        public string Name { get; set; }

        public List<Menu> Menu { get; set; }
    }
}
