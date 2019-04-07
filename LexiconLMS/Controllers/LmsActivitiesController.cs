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
    public class LmsActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LmsActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LmsActivities
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LmsActivity.Include(l => l.ActivityType).Include(l => l.Module);
            return View(await applicationDbContext.ToListAsync());
        }

        //Get: ModuleLmsActivities
        public async Task<IActionResult> ModuleActivities(int? moduleId)
        {
            var applicationDbContext = _context.LmsActivity.Include(l => l.ActivityType).Include(m => m.Module).Where(m => m.Module.Id == moduleId);
            //return View("Index", await applicationDbContext.ToListAsync());

            var module = _context.Module.Find(moduleId);

            ViewBag.ModuleName = module.Name;
            ViewBag.CourseId = module.CourseId;
            ViewBag.ModuleId = moduleId;

            //var course = _context.Course.Find(module.CourseId);
            //course.Name

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
        [Authorize(Roles = "Teacher")]
        public IActionResult Create()
        {
            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Type");
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Name");
            return View();
        }

        // POST: LmsActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EndDate,StartDate,Description,Name,ActivityTypeId,ModuleId")] LmsActivity lmsActivity)
        {
            var module = await _context.Module.FindAsync(lmsActivity.ModuleId);

            if ((lmsActivity.StartDate.Date >= module.StartDate.Date && lmsActivity.EndDate.Date <= module.EndDate.Date) && module.StartDate.Date <= module.EndDate.Date)
            {

                if (ModelState.IsValid)
                {
                    _context.Add(lmsActivity);
                    await _context.SaveChangesAsync();
                    TempData["SuccessText"] = $"Aktivitet: {lmsActivity.Name} skapades Ok!";
                    return RedirectToAction(nameof(Index));
                }
                TempData["FailText"] = $"Något gick fel vid skapandet av aktiviteten. Försök igen.";
                ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Type", lmsActivity.ActivityTypeId);
                ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Name", lmsActivity.ModuleId);
                return View(lmsActivity);
            }
            else
            {
                TempData["FailText"] = $"Startdatum och slutdatum måste ligga inom kursens start- och slutdatum!";
                ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Type", lmsActivity.ActivityTypeId);
                ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Name", lmsActivity.ModuleId);
                return View(module);
            }
        }


        //Get: LmsActivities/Create ModuleActivity
        [Authorize(Roles = "Teacher")]
        public IActionResult CreateModuleActivity(int moduleId)
        {
            var module = _context.Module.Find(moduleId);

            LmsActivity model = new LmsActivity
            {
                ModuleId = moduleId,
                StartDate = module.StartDate,
                EndDate = module.EndDate,
                Module = module
            };

            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Type");
            //ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Id");
            return base.View(model);
        }

        // POST: LmsActivities/Create ModuleActivity
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateModuleActivity([Bind("Id,EndDate,StartDate,Description,Name,ActivityTypeId,ModuleId")] LmsActivity lmsActivity)
        {
            var module = await _context.Module.FindAsync(lmsActivity.ModuleId);

            if ((lmsActivity.StartDate.Date >= module.StartDate.Date && lmsActivity.EndDate.Date <= module.EndDate.Date) && lmsActivity.StartDate.Date <= lmsActivity.EndDate.Date)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(lmsActivity);
                    await _context.SaveChangesAsync();
                    TempData["SuccessText"] = $"Aktivitet: {lmsActivity.Name} skapades Ok!";
                    return RedirectToAction(nameof(ModuleActivities), new { lmsActivity.ModuleId });
                }
                TempData["FailText"] = $"Något gick fel vid skapandet av aktiviteten. Försök igen.";
                ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Type", lmsActivity.ActivityTypeId);
                //ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Id", lmsActivity.ModuleId);
                lmsActivity.Module = module;
                return View(lmsActivity);
            }
            else
            {
                TempData["FailText"] = "Startdatum och slutdatum måste ligga inom modulens start- och slutdatum\n" +
                    "och startdatum kan inte ligga senare än slutdatum !";
                ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Type", lmsActivity.ActivityTypeId);
                //ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Id", lmsActivity.ModuleId);
                lmsActivity.Module = module;
                return View(lmsActivity);
            }
        }


        // GET: LmsActivities/Edit/5
        [Authorize(Roles = "Teacher")]
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

            var module = await _context.Module.FindAsync(lmsActivity.ModuleId);
            lmsActivity.Module = module;

            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Type", lmsActivity.ActivityTypeId);
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Name", lmsActivity.ModuleId);
            return View(lmsActivity);
        }

        // POST: LmsActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EndDate,StartDate,Description,Name,ActivityTypeId,ModuleId")] LmsActivity lmsActivity)
        {
            if (id != lmsActivity.Id)
            {
                return NotFound();
            }

            var module = await _context.Module.FindAsync(lmsActivity.ModuleId);

            if ((lmsActivity.StartDate.Date >= module.StartDate.Date && lmsActivity.EndDate.Date <= module.EndDate.Date) && lmsActivity.StartDate.Date <= lmsActivity.EndDate.Date)
            {

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
                    //return RedirectToAction(nameof(Index));
                    TempData["SuccessText"] = $"Aktivitet: {lmsActivity.Name} uppdaterad Ok!";
                    return RedirectToAction(nameof(ModuleActivities), new { lmsActivity.ModuleId });
                }
                ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Type", lmsActivity.ActivityTypeId);
                ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Name", lmsActivity.ModuleId);
                TempData["FailText"] = $"Något gick fel. Aktivitet: {lmsActivity.Name} uppdaterades inte!";
                lmsActivity.Module = module;
                return View(lmsActivity);
            }
            else
            {
                TempData["FailText"] = "Startdatum och slutdatum måste ligga inom modulens start- och slutdatum/n" +
                        "och startdatum kan inte ligga senare än slutdatum!";
                ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Type", lmsActivity.ActivityTypeId);
                ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Name", lmsActivity.ModuleId);
                lmsActivity.Module = module;
                return View(lmsActivity);
            }
        }


        // GET: LmsActivities/SubmitTask/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> SubmitTask(int? id)
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


            var studentmodule = await _context.Module.FindAsync(lmsActivity.ModuleId);
            var studentcourse = await _context.Course.FindAsync(studentmodule.CourseId);
            var studentspercourse = _context.Course.Include(a => a.ApplicationUser).
                FirstOrDefault(u => u.Id == studentcourse.Id);
            var submitTask = new SubmitTaskViewModel();
            submitTask.StudentCourse = studentcourse;
            submitTask.StudentModule = studentmodule;
            submitTask.LmsActivity = await _context.LmsActivity.FindAsync(id);


            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Type", lmsActivity.ActivityTypeId);
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Name", lmsActivity.ModuleId);
            return View(submitTask);
        }

        // POST: LmsActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitTask(int id)
        {
            


            var lmsActivity = await _context.LmsActivity.FindAsync(id);
            if (lmsActivity == null)
            {
                return NotFound();
            }


            var studentmodule = await _context.Module.FindAsync(lmsActivity.ModuleId);
            var studentcourse = await _context.Course.FindAsync(studentmodule.CourseId);
            var studentspercourse = _context.Course.Include(a => a.ApplicationUser).
                FirstOrDefault(u => u.Id == studentcourse.Id);
            var submitTask = new SubmitTaskViewModel();
            submitTask.StudentCourse = studentcourse;
            submitTask.StudentModule = studentmodule;
           
            submitTask.LmsActivity = await _context.LmsActivity.FindAsync(id);

            if (id != submitTask.LmsActivity.Id)
            {
                return NotFound();
            }

            //var LmsActivity = await _context.LmsActivity.FindAsync(submitTask.LmsActivity.Id);
            var taskList = new List<LmsTask>();
            var ReadyStatex = await _context.ReadyState.FirstOrDefaultAsync(u=>u.Type=="Inte Påbörjad");
            var ReadyStateId = ReadyStatex.Id;
            foreach (var student in studentcourse.ApplicationUser)
            {
                var studentTask = new LmsTask();
               
                studentTask.LmsActivityId = submitTask.LmsActivity.Id;
                studentTask.ApplicationUserId = student.Id;
                studentTask.TeacherDescription = submitTask.LmsActivity.Description;
                studentTask.ReadyStateId = ReadyStateId;


                taskList.Add(studentTask);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.LmsTask.AddRange(taskList);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LmsActivityExists(submitTask.LmsActivity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                TempData["SuccessText"] = $"Uppgifter till: {submitTask.LmsActivity.Name} skapad Ok!";
                return RedirectToAction(nameof(ModuleActivities), new { submitTask.LmsActivity.ModuleId });

                //ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Type", lmsActivity.ActivityTypeId);
                //ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Name", lmsActivity.ModuleId);
                //TempData["FailText"] = $"Något gick fel. Aktivitet: {lmsActivity.Name} uppdaterades inte!";
                //lmsActivity.Module = module;
                return View(submitTask.LmsActivity);
            }
            else
            {
                TempData["FailText"] = "Uppgifter ej skapade";
                //ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Type", lmsActivity.ActivityTypeId);
                //ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Name", lmsActivity.ModuleId);
             

                return View(submitTask.LmsActivity);
            }
        }




        // GET: LmsActivities/Delete/5
        [Authorize(Roles = "Teacher")]
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
        [Authorize(Roles = "Teacher")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lmsActivity = await _context.LmsActivity.FindAsync(id);
            _context.LmsActivity.Remove(lmsActivity);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            TempData["SuccessText"] = $"Aktivitet: {lmsActivity.Name} raderad Ok!";
            return RedirectToAction(nameof(ModuleActivities), new { lmsActivity.ModuleId });
        }

        private bool LmsActivityExists(int id)
        {
            return _context.LmsActivity.Any(e => e.Id == id);
        }
    }
}
