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

    public class WhyusController : Controller
    {
        private readonly AppDbContext _context;

        public WhyusController(AppDbContext context)
        {
            _context = context;
        }

        // GET: admin/Whyus
        [Route("/Whyus/Index")]
        public async Task<IActionResult> Index()
        {
              return _context.Whyus != null ? 
                          View(await _context.Whyus.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Whyus'  is null.");
        }

        // GET: admin/Whyus/Details/5
        [Route("/Whyus/Details")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Whyus == null)
            {
                return NotFound();
            }

            var whyus = await _context.Whyus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (whyus == null)
            {
                return NotFound();
            }

            return View(whyus);
        }

        // GET: admin/Whyus/Create
        [Route("/Whyus/Create")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/Whyus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Whyus/Create")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Uptitle,Title")] Whyus whyus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(whyus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(whyus);
        }

        // GET: admin/Whyus/Edit/5
        [Route("/Whyus/Edit")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Whyus == null)
            {
                return NotFound();
            }

            var whyus = await _context.Whyus.FindAsync(id);
            if (whyus == null)
            {
                return NotFound();
            }
            return View(whyus);
        }

        // POST: admin/Whyus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("/Whyus/Edit")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Uptitle,Title")] Whyus whyus)
        {
            if (id != whyus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(whyus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WhyusExists(whyus.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(whyus);
        }


        // GET: admin/Whyus/Delete/5
        [Route("/Whyus/Delete")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Whyus == null)
            {
                return NotFound();
            }

            var whyus = await _context.Whyus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (whyus == null)
            {
                return NotFound();
            }

            return View(whyus);
        }

        // POST: admin/Whyus/Delete/5
        [Route("/Whyus/Delete")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Whyus == null)
            {
                return Problem("Entity set 'AppDbContext.Whyus'  is null.");
            }
            var whyus = await _context.Whyus.FindAsync(id);
            if (whyus != null)
            {
                _context.Whyus.Remove(whyus);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WhyusExists(int id)
        {
          return (_context.Whyus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
