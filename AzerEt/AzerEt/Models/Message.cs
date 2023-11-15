using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AzerEt.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreateTime { get; set; }
        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        public CustomUser Sender { get; set; }
        [ForeignKey("Reciver")]
        public string ReciverId { get; set; }
        public CustomUser Reciver { get; set; }
    }
}
