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
    public class LmsTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LmsTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LmsTasks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LmsTask.Include(l => l.ApplicationUser).Include(l => l.LmsActivity).Include(l => l.ReadyState);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LmsTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lmsTask = await _context.LmsTask
                .Include(l => l.ApplicationUser)
                .Include(l => l.LmsActivity)
                .Include(l => l.ReadyState)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lmsTask == null)
            {
                return NotFound();
            }

            return View(lmsTask);
        }

        // GET: LmsTasks/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["LmsActivityId"] = new SelectList(_context.LmsActivity, "Id", "Id");
            ViewData["ReadyStateId"] = new SelectList(_context.Set<ReadyState>(), "Id", "Id");
            return View();
        }

        // POST: LmsTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TeacherFeedback,ReadyDate,StudentAnswer,StudentComment,ReadyStateId,LmsActivityId,ApplicationUserId")] LmsTask lmsTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lmsTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", lmsTask.ApplicationUserId);
            ViewData["LmsActivityId"] = new SelectList(_context.LmsActivity, "Id", "Id", lmsTask.LmsActivityId);
            ViewData["ReadyStateId"] = new SelectList(_context.Set<ReadyState>(), "Id", "Id", lmsTask.ReadyStateId);
            return View(lmsTask);
        }

        // GET: LmsTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lmsTask = await _context.LmsTask.FindAsync(id);
            if (lmsTask == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", lmsTask.ApplicationUserId);
            ViewData["LmsActivityId"] = new SelectList(_context.LmsActivity, "Id", "Id", lmsTask.LmsActivityId);
            ViewData["ReadyStateId"] = new SelectList(_context.Set<ReadyState>(), "Id", "Id", lmsTask.ReadyStateId);
            return View(lmsTask);
        }

        // POST: LmsTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeacherFeedback,ReadyDate,StudentAnswer,StudentComment,ReadyStateId,LmsActivityId,ApplicationUserId")] LmsTask lmsTask)
        {
            if (id != lmsTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lmsTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LmsTaskExists(lmsTask.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", lmsTask.ApplicationUserId);
            ViewData["LmsActivityId"] = new SelectList(_context.LmsActivity, "Id", "Id", lmsTask.LmsActivityId);
            ViewData["ReadyStateId"] = new SelectList(_context.Set<ReadyState>(), "Id", "Id", lmsTask.ReadyStateId);
            return View(lmsTask);
        }
        //Get: ActivityTasks
        public async Task<IActionResult> ActivityTasks(int? lmsActivityId)
        {
            var applicationDbContext = _context.LmsTask.Include(l => l.ReadyState).Include(m => m.LmsActivity).Where(m => m.LmsActivity.Id == lmsActivityId);
            //return View("Index", await applicationDbContext.ToListAsync());
            var lmsActivity = _context.LmsActivity.Find(lmsActivityId);
            var module = _context.Module.Find(lmsActivity.ModuleId);

            ViewBag.ModuleName = module.Name;
            ViewBag.CourseId = module.CourseId;
            ViewBag.ModuleId = lmsActivity.ModuleId;

            //var course = _context.Course.Find(module.CourseId);
            //course.Name

            return View(await applicationDbContext.ToListAsync());
        }
        // GET: LmsTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lmsTask = await _context.LmsTask
                .Include(l => l.ApplicationUser)
                .Include(l => l.LmsActivity)
                .Include(l => l.ReadyState)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lmsTask == null)
            {
                return NotFound();
            }

            return View(lmsTask);
        }

        // POST: LmsTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lmsTask = await _context.LmsTask.FindAsync(id);
            _context.LmsTask.Remove(lmsTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: LmsTasks/Report/5
        public async Task<IActionResult> ReportTask(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var lmsTask = await _context.LmsTask.FindAsync(Id);
            if (lmsTask == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = lmsTask.ApplicationUserId;
            ViewData["LmsActivityId"] = lmsTask.LmsActivityId;
            ViewData["ReadyStateId"] = new SelectList(_context.Set<ReadyState>(), "Id", "Id", lmsTask.ReadyStateId);
            return View(lmsTask);
        }

        // POST: LmsTasks/ReortTask/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReportTask(int Id, [Bind("Id,TeacherFeedback,ReadyDate,StudentAnswer,StudentComment,ReadyStateId,LmsActivityId,ApplicationUserId")] LmsTask lmsTask)
        {
            if (Id != lmsTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lmsTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LmsTaskExists(lmsTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Student", "Courses");
            }
            ViewData["ApplicationUserId"] = lmsTask.ApplicationUserId;
            ViewData["LmsActivityId"] = lmsTask.LmsActivityId;
            ViewData["ReadyStateId"] = new SelectList(_context.Set<ReadyState>(), "Id", "Id", lmsTask.ReadyState);
            return View(lmsTask);
        }



        private bool LmsTaskExists(int id)
        {
            return _context.LmsTask.Any(e => e.Id == id);
        }
    }
}
