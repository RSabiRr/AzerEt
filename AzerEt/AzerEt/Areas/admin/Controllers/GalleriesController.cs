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

    public class GalleriesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GalleriesController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: admin/Galleries
        [Route("/Galleries/Index")]
        public async Task<IActionResult> Index()
        {
              return _context.Galleries != null ? 
                          View(await _context.Galleries.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Galleries'  is null.");
        }

        // GET: admin/Galleries/Details/5
        [Route("/Galleries/Details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Galleries == null)
            {
                return NotFound();
            }

            var gallery = await _context.Galleries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gallery == null)
            {
                return NotFound();
            }

            return View(gallery);
        }

        // GET: admin/Galleries/Create
        [Route("/Galleries/Create")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/Galleries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Galleries/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Gallery gallery)
        {
            if (gallery.ImageFile != null)
            {
                if (gallery.ImageFile.ContentType == "image/jpeg" || gallery.ImageFile.ContentType == "image/png")
                {
                    if (gallery.ImageFile.Length <= 3000000)
                    {
                        string FileName = Guid.NewGuid() + "-" + gallery.ImageFile.FileName;
                        string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "UploadsGallery", FileName);
                        using (var stream = new FileStream(FilePath, FileMode.Create))
                        {
                            gallery.ImageFile.CopyTo(stream);
                        }
                        gallery.Image = FileName;
                        _context.Galleries.Add(gallery);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        ModelState.AddModelError("", "you can choose only 3 mb image file");
                        return View(gallery);
                    }


                }
                else
                {
                    ModelState.AddModelError("", "you can choose only image file");
                    return View(gallery);

                }

            }
            else
            {
                ModelState.AddModelError("", " choose image file");
                return View(gallery);

            }



            return RedirectToAction(nameof(Index));
        }

        // GET: admin/Galleries/Edit/5
        [Route("/Galleries/Edit")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Galleries == null)
            {
                return NotFound();
            }

            var gallery = await _context.Galleries.FindAsync(id);
            if (gallery == null)
            {
                return NotFound();
            }
            return View(gallery);
        }

        // POST: admin/Galleries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Galleries/Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Gallery gallery)
        {
            if (gallery.ImageFile != null)
            {
                if (gallery.ImageFile.ContentType == "image/jpeg" || gallery.ImageFile.ContentType == "image/png")
                {
                    if (gallery.ImageFile.Length <= 3000000)
                    {
                        string olddata = Path.Combine(_webHostEnvironment.WebRootPath, "UploadsGallery", gallery.Image);
                        if (System.IO.File.Exists(olddata))
                        {
                            System.IO.File.Delete(olddata);
                        }
                        string FileName = Guid.NewGuid() + "-" + gallery.ImageFile.FileName;
                        string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "UploadsGallery", FileName);
                        using (var stream = new FileStream(FilePath, FileMode.Create))
                        {
                            gallery.ImageFile.CopyTo(stream);
                        }
                        gallery.Image = FileName;
                        _context.Galleries.Update(gallery);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        ModelState.AddModelError("", "you can choose only 3 mb image file");
                        return View(gallery);
                    }


                }
                else
                {
                    ModelState.AddModelError("", "you can choose only image file");
                    return View(gallery);

                }

            }
            else
            {
                ModelState.AddModelError("", " choose image file");
                return View(gallery);

            }



            return RedirectToAction(nameof(Index));
        }

        // GET: admin/Galleries/Delete/5
        [Route("/Galleries/Delete")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Galleries == null)
            {
                return NotFound();
            }

            var gallery = await _context.Galleries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gallery == null)
            {
                return NotFound();
            }

            return View(gallery);
        }

        // POST: admin/Galleries/Delete/5
        [Route("/Galleries/Delete")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Galleries == null)
            {
                return Problem("Entity set 'AppDbContext.Galleries'  is null.");
            }
            var gallery = await _context.Galleries.FindAsync(id);
            if (gallery != null)
            {
                string olddata = Path.Combine(_webHostEnvironment.WebRootPath, "UploadsGallery", gallery.Image);
                if (System.IO.File.Exists(olddata))
                {
                    System.IO.File.Delete(olddata);
                }
                _context.Galleries.Remove(gallery);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GalleryExists(int id)
        {
          return (_context.Galleries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
