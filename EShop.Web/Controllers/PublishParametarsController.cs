using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EShop.Domain.Domain;
using EShop.Repository;

namespace EShop.Web.Controllers
{
    public class PublishParametarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublishParametarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PublishParametars
        public async Task<IActionResult> Index()
        {
            return View(await _context.PublishParametars.ToListAsync());
        }

        // GET: PublishParametars/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publishParametars = await _context.PublishParametars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publishParametars == null)
            {
                return NotFound();
            }

            return View(publishParametars);
        }

        // GET: PublishParametars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PublishParametars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MinimumRentDays,Discount,Id")] PublishParametars publishParametars)
        {
            if (ModelState.IsValid)
            {
                publishParametars.Id = Guid.NewGuid();
                _context.Add(publishParametars);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publishParametars);
        }

        // GET: PublishParametars/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publishParametars = await _context.PublishParametars.FindAsync(id);
            if (publishParametars == null)
            {
                return NotFound();
            }
            return View(publishParametars);
        }

        // POST: PublishParametars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("MinimumRentDays,Discount,Id")] PublishParametars publishParametars)
        {
            if (id != publishParametars.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publishParametars);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublishParametarsExists(publishParametars.Id))
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
            return View(publishParametars);
        }

        // GET: PublishParametars/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publishParametars = await _context.PublishParametars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publishParametars == null)
            {
                return NotFound();
            }

            return View(publishParametars);
        }

        // POST: PublishParametars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var publishParametars = await _context.PublishParametars.FindAsync(id);
            if (publishParametars != null)
            {
                _context.PublishParametars.Remove(publishParametars);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublishParametarsExists(Guid id)
        {
            return _context.PublishParametars.Any(e => e.Id == id);
        }
    }
}
