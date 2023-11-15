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

    public class SettingsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SettingsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: admin/Settings
        [Route("/Settings/Index")]

        public async Task<IActionResult> Index()
        {
              return _context.Settings != null ? 
                          View(await _context.Settings.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Settings'  is null.");
        }

        // GET: admin/Settings/Details/5
        [Route("/Settings/Details")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Settings == null)
            {
                return NotFound();
            }

            var setting = await _context.Settings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (setting == null)
            {
                return NotFound();
            }

            return View(setting);
        }

        // GET: admin/Settings/Create
        [Route("/Settings/Create")]


        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/Settings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Settings/Create")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Setting setting)
        {
            if (setting.ImageFile != null)
            {
                if (setting.ImageFile.ContentType == "image/jpeg" || setting.ImageFile.ContentType == "image/png")
                {
                    if (setting.ImageFile.Length <= 3000000)
                    {
                        string FileName = Guid.NewGuid() + "-" + setting.ImageFile.FileName;
                        string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "UploadsSetting", FileName);
                        using (var stream = new FileStream(FilePath, FileMode.Create))
                        {
                            setting.ImageFile.CopyTo(stream);
                        }
                        setting.Logo = FileName;
                        _context.Settings.Add(setting);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        ModelState.AddModelError("", "you can choose only 3 mb image file");
                        return View(setting);
                    }


                }
                else
                {
                    ModelState.AddModelError("", "you can choose only image file");
                    return View(setting);

                }

            }
            else
            {
                ModelState.AddModelError("", " choose image file");
                return View(setting);

            }



            return RedirectToAction(nameof(Index));
        }

        // GET: admin/Settings/Edit/5
        [Route("/Settings/Edit")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Settings == null)
            {
                return NotFound();
            }

            var setting = await _context.Settings.FindAsync(id);
            if (setting == null)
            {
                return NotFound();
            }
            return View(setting);
        }

        // POST: admin/Settings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Settings/Edit")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Setting setting)
        {
            if (setting.ImageFile != null)
            {
                if (setting.ImageFile.ContentType == "image/jpeg" || setting.ImageFile.ContentType == "image/png")
                {
                    if (setting.ImageFile.Length <= 3000000)
                    {
                        string olddata = Path.Combine(_webHostEnvironment.WebRootPath, "UploadsSetting", setting.Logo);
                        if (System.IO.File.Exists(olddata))
                        {
                            System.IO.File.Delete(olddata);
                        }
                        string FileName = Guid.NewGuid() + "-" + setting.ImageFile.FileName;
                        string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "UploadsSetting", FileName);
                        using (var stream = new FileStream(FilePath, FileMode.Create))
                        {
                            setting.ImageFile.CopyTo(stream);
                        }
                        setting.Logo = FileName;
                        _context.Settings.Update(setting);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        ModelState.AddModelError("", "you can choose only 3 mb image file");
                        return View(setting);
                    }


                }
                else
                {
                    ModelState.AddModelError("", "you can choose only image file");
                    return View(setting);

                }

            }
            else
            {
                ModelState.AddModelError("", " choose image file");
                return View(setting);

            }



            return RedirectToAction(nameof(Index));
        }

        // GET: admin/Settings/Delete/5
        [Route("/Settings/Delete")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Settings == null)
            {
                return NotFound();
            }

            var setting = await _context.Settings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (setting == null)
            {
                return NotFound();
            }

            return View(setting);
        }

        // POST: admin/Settings/Delete/5
        [Route("/Settings/Delete")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Settings == null)
            {
                return Problem("Entity set 'AppDbContext.Settings'  is null.");
            }
            var setting = await _context.Settings.FindAsync(id);
            if (setting != null)
            {
                string olddata = Path.Combine(_webHostEnvironment.WebRootPath, "UploadsSetting", setting.Logo);
                if (System.IO.File.Exists(olddata))
                {
                    System.IO.File.Delete(olddata);
                }
                _context.Settings.Remove(setting);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SettingExists(int id)
        {
          return (_context.Settings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
