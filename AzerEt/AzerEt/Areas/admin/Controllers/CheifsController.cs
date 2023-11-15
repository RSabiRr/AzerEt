using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzerEt.Data;
using AzerEt.Models;
using Microsoft.AspNetCore.Authorization;

namespace AzerEt.Areas.admin.Controllers
{
    //[Authorize(Roles = "Admin")]

    [Area("admin")]
    [Authorize(Roles = "Admin")]

    public class CheifsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CheifsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: admin/Cheifs
        [Route("/Cheifs/Index")]
        public async Task<IActionResult> Index()
        {
              return _context.Cheifs != null ? 
                          View(await _context.Cheifs.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Cheifs'  is null.");
        }

        // GET: admin/Cheifs/Details/5
        [Route("/Cheifs/Details")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cheifs == null)
            {
                return NotFound();
            }

            var cheif = await _context.Cheifs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cheif == null)
            {
                return NotFound();
            }

            return View(cheif);
        }

        // GET: admin/Cheifs/Create
        [Route("/Cheifs/Create")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/Cheifs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Cheifs/Create")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Cheif cheif)
        {
            if (cheif.ImageFile != null)
            {
                if (cheif.ImageFile.ContentType == "image/jpeg" || cheif.ImageFile.ContentType == "image/png")
                {
                    if (cheif.ImageFile.Length <= 3000000)
                    {
                        string FileName = Guid.NewGuid() + "-" + cheif.ImageFile.FileName;
                        string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "UploadsCheifs", FileName);
                        using (var stream = new FileStream(FilePath, FileMode.Create))
                        {
                            cheif.ImageFile.CopyTo(stream);
                        }
                        cheif.Image = FileName;
                        _context.Cheifs.Add(cheif);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        ModelState.AddModelError("", "you can choose only 3 mb image file");
                        return View(cheif);
                    }


                }
                else
                {
                    ModelState.AddModelError("", "you can choose only image file");
                    return View(cheif);

                }

            }
            else
            {
                ModelState.AddModelError("", " choose image file");
                return View(cheif);

            }



            return RedirectToAction(nameof(Index));
        }

        // GET: admin/Cheifs/Edit/5
        [Route("/Cheifs/Edit")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cheifs == null)
            {
                return NotFound();
            }

            var cheif = await _context.Cheifs.FindAsync(id);
            if (cheif == null)
            {
                return NotFound();
            }
            return View(cheif);
        }

        // POST: admin/Cheifs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Cheifs/Edit")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Cheif cheif)
        {
            if (cheif.ImageFile != null)
            {
                if (cheif.ImageFile.ContentType == "image/jpeg" || cheif.ImageFile.ContentType == "image/png")
                {
                    if (cheif.ImageFile.Length <= 3000000)
                    {
                        string olddata = Path.Combine(_webHostEnvironment.WebRootPath, "UploadsCheifs", cheif.Image);
                        if (System.IO.File.Exists(olddata))
                        {
                            System.IO.File.Delete(olddata);
                        }
                        string FileName = Guid.NewGuid() + "-" + cheif.ImageFile.FileName;
                        string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "UploadsCheifs", FileName);
                        using (var stream = new FileStream(FilePath, FileMode.Create))
                        {
                            cheif.ImageFile.CopyTo(stream);
                        }
                        cheif.Image = FileName;
                        _context.Cheifs.Update(cheif);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        ModelState.AddModelError("", "you can choose only 3 mb image file");
                        return View(cheif);
                    }


                }
                else
                {
                    ModelState.AddModelError("", "you can choose only image file");
                    return View(cheif);

                }

            }
            else
            {
                ModelState.AddModelError("", " choose image file");
                return View(cheif);

            }



            return RedirectToAction(nameof(Index));
        }

        // GET: admin/Cheifs/Delete/5
        [Route("/Cheifs/Delete")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cheifs == null)
            {
                return NotFound();
            }

            var cheif = await _context.Cheifs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cheif == null)
            {
                return NotFound();
            }

            return View(cheif);
        }

        // POST: admin/Cheifs/Delete/5
        [Route("/Cheifs/Delete")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cheifs == null)
            {
                return Problem("Entity set 'AppDbContext.Cheifs'  is null.");
            }
            var cheif = await _context.Cheifs.FindAsync(id);
            if (cheif != null)
            {
                string olddata = Path.Combine(_webHostEnvironment.WebRootPath, "UploadsCheifs", cheif.Image);
                if (System.IO.File.Exists(olddata))
                {
                    System.IO.File.Delete(olddata);
                }
                _context.Cheifs.Remove(cheif);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheifExists(int id)
        {
          return (_context.Cheifs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
