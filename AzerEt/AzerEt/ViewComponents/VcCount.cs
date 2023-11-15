using AzerEt.Data;
using Microsoft.AspNetCore.Mvc;

namespace AzerEt.ViewComponents
{
    public class VcCount : ViewComponent
    {

        private readonly AppDbContext _context;


        public VcCount(AppDbContext context)
        {
            _context = context;

        }
        public IViewComponentResult Invoke()
        {
            return View(_context.Checkouts.ToList());

        }
    }



}
