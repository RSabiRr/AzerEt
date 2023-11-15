using AzerEt.Data;
using AzerEt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzerEt.Areas.admin.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        //[Authorize(Roles = "Admin")]
        [Area("admin")]
        [Authorize(Roles = "Admin")]

        [Route("admin")]
        public IActionResult Index()
        {


            VmHome model = new VmHome()
            {
                Setting = _context.Settings.FirstOrDefault(),
                CustomUserss=_context.CustomUsers.ToList(),
                Menus = _context.Menus.ToList(),
                Wishlists = _context.Wishlists.ToList(),
                Categories = _context.Categories.ToList(),
                Specials = _context.Specials.ToList(),
                Checkouts=_context.Checkouts.ToList(),
                Galleries = _context.Galleries.ToList(),
                Whyus = _context.Whyus.ToList(),
                Contactss=_context.Contacts.ToList(),
                Cheifs=_context.Cheifs.ToList()


            };
            return View(model);
        }
    }
}
