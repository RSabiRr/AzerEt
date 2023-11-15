using AzerEt.Data;
using Microsoft.AspNetCore.Mvc;

namespace AzerEt.ViewComponents
{
    public class VcSettingcs:ViewComponent
    {

        private readonly AppDbContext _context;


        public VcSettingcs(AppDbContext context)
        {
            _context = context;

        }
        public IViewComponentResult Invoke()
        {
            return View(_context.Settings.FirstOrDefault());

        }
    
    }
}
