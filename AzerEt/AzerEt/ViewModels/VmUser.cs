using AzerEt.Models;

namespace AzerEt.ViewModels
{
    public class VmUser
    {
        public List<CustomUser> CustomUsers { get; set; }
        public Dictionary<string, string> UserRoles { get; set; }
        public int PageCount { get; set; }
        public double ItemCount { get; set; }
        public int Page { get; set; }
    }
}
