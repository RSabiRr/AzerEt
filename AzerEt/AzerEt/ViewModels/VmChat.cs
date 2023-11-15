using AzerEt.Models;

namespace AzerEt.ViewModels
{
    public class VmChat
    {
        public CustomUser Reciever { get; set; }
        public List<Message> Messages { get; set; }
        public Message Message { get; set; }

        public List<CustomUser> CustomUsers { get; set; }




    }
}

