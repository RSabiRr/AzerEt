using AzerEt.Data;
using AzerEt.Models;
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzerEt.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class WishlistController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public WishlistController(AppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private static readonly List<CustomUser> items = new List<CustomUser>();


        [ActionName("ChatCount")]
        [HttpPost]
        public IActionResult ChatCount([FromBody] string senderid)
        {
            var id = senderid;

            var list = _context.CustomUsers.Where(m => m.Id == id).ToList().FirstOrDefault();

            if (senderid == null)
            {
                return BadRequest("Geçersiz veri.");
            }

            list.Count++;
            _context.SaveChanges();

            var add = new { success = true, message = "Count artdi" };
            return Json(add);
        }

        [HttpPost]
        public IActionResult CountDelete([FromBody] string senderid)
        {
            var id = senderid;

            var list = _context.CustomUsers.Where(m => m.Id == id).ToList().FirstOrDefault();

            if (senderid == null)
            {
                return BadRequest("Geçersiz veri.");
            }

            list.Count=0;
            _context.SaveChanges();

            var add = new { success = true, message = "Count silindi" };
            return Json(add);
        }
        [ActionName("post")]
        [HttpPost]
        public IActionResult Post([FromBody] Wishlist item,int menuId)
        {
            var id = item.UserId;
            var list = _context.Wishlists.Where(m => m.UserId == id).ToList();
            if (item == null)
            {
                return BadRequest("Geçersiz veri.");
            }

            if (list.Any(x => x.MenuId == item.MenuId))
            {

                var menyu = _context.Wishlists.Where(z=>z.MenuId==item.MenuId).FirstOrDefault();
                var contact = _context.Wishlists.Find(menyu.Id);
                 _context.Wishlists.Remove(contact);
                _context.SaveChanges();
                var sil = new { success = true, message = "İşlem başarıyla silindi." };
                return Json(sil);
            }
            else
            {
                _context.Add(item);
                _context.SaveChanges();
            }

            var add = new { success = true, message = "İşlem başarıyla tamamlandı." };
            return Json(add);
        }

         
        [HttpGet]
        public IActionResult Get( Wishlist itemm)
        {
            var id = itemm.UserId;
            var menuid = itemm.MenuId;
            var data = _context.Wishlists.Where(w=>w.UserId==id).ToList();

            return Json(data);
        }


        [HttpGet]
        public IActionResult GetData()
        {
            var data = _context.CustomUsers.Where(m=>m.Count!=0).ToList().Select(m=>m.Count);
            return Json(data);
        }


    }
}
