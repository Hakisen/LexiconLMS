using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LexiconLMS.Data;
using LexiconLMS.Models;

namespace LexiconLMS.Controllers
{
    public class LmsActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LmsActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LmsActivities
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LmsActivity.Include(l => l.ActivityType).Include(l => l.Module);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LmsActivities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lmsActivity = await _context.LmsActivity
                .Include(l => l.ActivityType)
                .Include(l => l.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lmsActivity == null)
            {
                return NotFound();
            }

            return View(lmsActivity);
        }

        // GET: LmsActivities/Create
        public IActionResult Create()
        {
            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Id");
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Id");
            return View();
        }

        // POST: LmsActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EndDate,StartDate,Description,Name,ActivityTypeId,ModuleId")] LmsActivity lmsActivity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lmsActivity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Id", lmsActivity.ActivityTypeId);
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Id", lmsActivity.ModuleId);
            return View(lmsActivity);
        }

        // GET: LmsActivities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lmsActivity = await _context.LmsActivity.FindAsync(id);
            if (lmsActivity == null)
            {
                return NotFound();
            }
            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Id", lmsActivity.ActivityTypeId);
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Id", lmsActivity.ModuleId);
            return View(lmsActivity);
        }

        // POST: LmsActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EndDate,StartDate,Description,Name,ActivityTypeId,ModuleId")] LmsActivity lmsActivity)
        {
            if (id != lmsActivity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lmsActivity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LmsActivityExists(lmsActivity.Id))
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
            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Id", lmsActivity.ActivityTypeId);
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Id", lmsActivity.ModuleId);
            return View(lmsActivity);
        }

        // GET: LmsActivities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lmsActivity = await _context.LmsActivity
                .Include(l => l.ActivityType)
                .Include(l => l.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lmsActivity == null)
            {
                return NotFound();
            }

            return View(lmsActivity);
        }

        // POST: LmsActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lmsActivity = await _context.LmsActivity.FindAsync(id);
            _context.LmsActivity.Remove(lmsActivity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LmsActivityExists(int id)
        {
            return _context.LmsActivity.Any(e => e.Id == id);
        }
    }
}
