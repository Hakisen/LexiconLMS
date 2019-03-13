using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LexiconLMS.Data;
using LexiconLMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace LexiconLMS.Controllers
{
    [Authorize]
    public class ModulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Modules
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Module.Include(c => c.Course);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> CourseModules(int? courseId)
        {
            var applicationDbContext = _context.Module.Include(c => c.Course).Where(c => c.Course.Id == courseId);
            //return View("Index", await applicationDbContext.ToListAsync());
            ViewBag.CourseName = _context.Course.Find(courseId).Name;
            ViewBag.CourseId = courseId;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Modules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _context.Module
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        // GET: Modules/Create
        [Authorize(Roles = "Teacher")]
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Name");
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EndDate,StartDate,Description,Name,CourseId")] Module module)
        {
            var course = await _context.Course.FindAsync(module.CourseId);

            if (module.StartDate.Date >= course.StartDate.Date && module.EndDate.Date <= course.EndDate.Date)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(module);
                    await _context.SaveChangesAsync();
                    TempData["SuccessText"] = $"Modul: {module.Name} skapades Ok!";
                    return RedirectToAction(nameof(Index));
                }
                TempData["FailText"] = $"Något gick fel vid skapandet av modulen. Försök igen";
                ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Name", module.CourseId);
                //module.Course = course;
                return View(module);
            }
            else
            {
                TempData["FailText"] = $"Startdatum och slutdatum måste ligga inom kursens start- och slutdatum!";
                ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Name", module.CourseId);
                //module.Course = course;
                return View(module);
            }
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult CreateCourseModule(int courseId)
        {
            if (courseId == null)
            {
                return NotFound();
            }

            //ViewData["CourseId"] = courseId;
            //var CourseId = id;  //sätter Id (för module) till CourseId ????
            //ViewBag.ModuleCourseId = id;
            //ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Name");

            var course = _context.Course.Find(courseId);
            //ViewBag.CourseName = course.Name; behövs ej nu när vi inkluderar en Course = course i modellen (Module)

            Module model = new Module {
                CourseId = courseId,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                Course = course
            };

            return base.View(model);
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourseModule([Bind("Id,EndDate,StartDate,Description,Name,CourseId")] Module module)
        {

            var course = await _context.Course.FindAsync(module.CourseId);

            if ((module.StartDate.Date >= course.StartDate.Date && module.EndDate.Date <= course.EndDate.Date) && module.StartDate.Date <= module.EndDate.Date)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(module);
                    await _context.SaveChangesAsync();
                    TempData["SuccessText"] = $"Modul: {module.Name} skapades Ok!";
                    return RedirectToAction(nameof(CourseModules), new { module.CourseId });
                }

                TempData["FailText"] = $"Något gick fel vid skapandet av modulen. Försök igen";
                //ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Name", module.CourseId);
                module.Course = course;
                return View(module);
            }

            else
            {
                TempData["FailText"] = "Startdatum och slutdatum måste ligga inom kursens start- och slutdatum/n" +
                                        "och startdatum kan inte ligga senare än slutdatum!";
                //ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Name", module.CourseId);
                module.Course = course;
                return View(module);
            }

            //tidigare koden (nedan) för ej Valid ModelState innan kollen av datum lagts till
            //ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Name", module.CourseId);
            //return View(module);
        }

        // GET: Modules/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _context.Module.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(module.CourseId);
            module.Course = course;

            //ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Name", module.CourseId);
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Name", module.CourseId);
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EndDate,StartDate,Description,Name,CourseId")] Module module)
        {
            if (id != module.Id)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(module.CourseId);

            if ((module.StartDate.Date >= course.StartDate.Date && module.EndDate.Date <= course.EndDate.Date) && module.StartDate.Date <= module.EndDate.Date)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(module);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ModuleExists(module.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    //return RedirectToAction(nameof(Index));
                    TempData["SuccessText"] = $"Modul: {module.Name} uppdaterad Ok!";
                    return RedirectToAction(nameof(CourseModules), new { module.CourseId });
                }
                ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Name", module.CourseId);
                TempData["FailText"] = $"Något gick fel. Modul: {module.Name} uppdaterades inte!";
                module.Course = course;
                return View(module);
            }
            else
            {
                TempData["FailText"] = "Startdatum och slutdatum måste ligga inom kursens start- och slutdatum/n" +
                                        "och startdatum kan inte ligga senare än slutdatum!";
                ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Name", module.CourseId);
                module.Course = course;
                return View(module);
            }

        }

        // GET: Modules/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _context.Module
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (module == null)
            {
                return NotFound();
            }
            //ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Name", module.CourseId);

            return View(module);
        }

        // POST: Modules/Delete/5
        [Authorize(Roles = "Teacher")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var module = await _context.Module.FindAsync(id);
            //var courseId = module.CourseId; behövs ej, objektet lever vidare så länge det är i scope
            //var moduleName = module.Name;
            _context.Module.Remove(module);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            TempData["SuccessText"] = $"Modul: {module.Name} raderad Ok!";
            return RedirectToAction(nameof(CourseModules), new { module.CourseId});
        }

        private bool ModuleExists(int id)
        {
            return _context.Module.Any(e => e.Id == id);
        }
    }
}
