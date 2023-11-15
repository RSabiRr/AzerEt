using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzerEt.Data;
using AzerEt.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace AzerEt.Areas.admin.Controllers
{
    //[Authorize(Roles = "Admin")]

    [Area("admin")]
    [Authorize(Roles = "Admin")]

    public class MenusController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MenusController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: admin/Menus
        [Route("/Menus/Index")]
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Menus.Include(m => m.Category);
            return View(await appDbContext.ToListAsync());
        }

        // GET: admin/Menus/Details/5
        [Route("/Menus/Details")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Menus == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: admin/Menus/Create
        [Route("/Menus/Create")]

        public IActionResult Create()
        {
            ViewBag.CategoryList = _context.Categories.ToList();
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        // POST: admin/Menus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Menus/Create")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Menu menu)
        {
            ViewBag.CategoryList = _context.Categories.ToList();

            if (menu.ImageFile != null)
                {
                    if (menu.ImageFile.ContentType == "image/jpeg" || menu.ImageFile.ContentType == "image/png")
                    {
                        if (menu.ImageFile.Length <= 3000000)
                        {
                            string FileName = Guid.NewGuid() + "-" + menu.ImageFile.FileName;
                            string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "UploadsImage", FileName);
                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                menu.ImageFile.CopyTo(stream);
                            }
                            menu.Image = FileName;
                            _context.Menus.Add(menu);
                            await _context.SaveChangesAsync();

                        }
                        else
                        {
                            ModelState.AddModelError("", "you can choose only 3 mb image file");
                            return View(menu);
                        }


                    }
                    else
                    {
                        ModelState.AddModelError("", "you can choose only image file");
                        return View(menu);

                    }

                }
                else
                {
                    ModelState.AddModelError("", " choose image file");
                    return View(menu);

                }



            return RedirectToAction(nameof(Index));

        }
    

        // GET: admin/Menus/Edit/5
        [Route("/Menus/Edit")]

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.CategoryList = _context.Categories.ToList();

            if (id == null || _context.Menus == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", menu.CategoryId);
            return View(menu);
        }

        // POST: admin/Menus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Menus/Edit")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Menu menu)
        {
            ViewBag.CategoryList = _context.Categories.ToList();
            if (menu.ImageFile != null)
            {
                if (menu.ImageFile.ContentType == "image/jpeg" || menu.ImageFile.ContentType == "image/png")
                {
                    if (menu.ImageFile.Length <= 3000000)
                    {
                        string olddata = Path.Combine(_webHostEnvironment.WebRootPath, "UploadsImage", menu.Image);
                        if (System.IO.File.Exists(olddata))
                        {
                            System.IO.File.Delete(olddata);
                        }
                        string FileName = Guid.NewGuid() + "-" + menu.ImageFile.FileName;
                        string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "UploadsImage", FileName);
                        using (var stream = new FileStream(FilePath, FileMode.Create))
                        {
                            menu.ImageFile.CopyTo(stream);
                        }
                        menu.Image = FileName;
                        _context.Menus.Update(menu);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        ModelState.AddModelError("", "you can choose only 3 mb image file");
                        return View(menu);
                    }


                }
                else
                {
                    ModelState.AddModelError("", "you can choose only image file");
                    return View(menu);

                }

            }
            else
            {
                ModelState.AddModelError("", " choose image file");
                return View(menu);

            }



            return RedirectToAction(nameof(Index));

        }

        // GET: admin/Menus/Delete/5
        [Route("/Menus/Delete")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Menus == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: admin/Menus/Delete/5
        [Route("/Menus/Delete")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Menus == null)
            {
                return Problem("Entity set 'AppDbContext.Menus'  is null.");
            }
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                string olddata = Path.Combine(_webHostEnvironment.WebRootPath, "UploadsImage", menu.Image);
                if (System.IO.File.Exists(olddata))
                {
                    System.IO.File.Delete(olddata);
                }
                _context.Menus.Remove(menu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
          return (_context.Menus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
