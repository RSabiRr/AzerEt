using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AzerEt.Models
{
    public class Cheif
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(250)]
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [MaxLength(250)]
        public string Fullname { get; set; }
        [MaxLength(250)]
        public string Position { get; set; }
        [MaxLength(250)]
        public string Facelink { get; set; }
        [MaxLength(250)]
        public string Instalink { get; set; }
        [MaxLength(250)]
        public string Linkedinlink { get; set; }


    }
}
