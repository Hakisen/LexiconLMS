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
using Microsoft.AspNetCore.Identity;
using LexiconLMS.Utility;

namespace LexiconLMS.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoursesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Courses
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Course.Include(a => a.ApplicationUser).ToListAsync());
        }
        // GET: Students per course in Teacher View
        [Authorize(Roles = "Teacher, Student")]
        public IActionResult StudentsPerCourse(int? id)
        {
            var studentspercourse = _context.Course.Include(a => a.ApplicationUser).
                 FirstOrDefault(u => u.Id == id);
            return View(studentspercourse);
        }

        //New landing page for logged-in students
        [Authorize(Roles = "teacher, Student")]
        public async Task<IActionResult> StudentHomePage()
        {
            var studentName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(studentName);

            ApplicationUser appUserStudent = user;
            ViewBag.AppUserStudentName = appUserStudent.Name; //ApplicationUser-Name

            ViewBag.StudentName = user.Name; //IdentityUser-Name = e-mail
            var courseId = (int)user.CourseId;

            var course = await _context.Course.FindAsync(courseId);
            ViewBag.CourseName = course.Name;


            //Fungerar, ger en Ienumerable av Modules
            var modules = _context.Module.Include(c => c.Course).Where(c => c.Course.Id == courseId).Where(m => m.StartDate.Date <= DateTime.Now.Date && m.EndDate.Date >= DateTime.Now.Date);

            var modules2 = _context.Module.Include(c => c.Course).Include(a => a.LmsActivities).Include(d => d.Documents).Where(c => c.Course.Id == courseId).Where(m => m.StartDate.Date <= DateTime.Now.Date && m.EndDate.Date >= DateTime.Now.Date);
            
            //Select ger en lista av <bool>???????
            //.Select(m => m.StartDate.Date <= DateTime.Now.Date && m.EndDate.Date >= DateTime.Now.Date);

            //var test = module.Where(m => m.StartDate.Date <= DateTime.Now.Date && m.EndDate >= DateTime.Now.Date);

            //Fungerar, ger första (enda) modulen
            var module = modules2.FirstOrDefault();
            var moduleId = module.Id;
            ViewBag.ModuleName = module.Name;
            //module.LmsActivities

            var moduleActivities = _context.LmsActivity.Include(a => a.ActivityType).Include(m => m.Module).Include(m => m.Module.Documents) .Where(m => m.Module.Id == moduleId);

            return View(moduleActivities);
        }

               

        // GET: Modules per course for Student View
        [Authorize(Roles = "Teacher, Student")]
        public async Task<IActionResult> StudentModules(int? id)
        {
            var student = new StudentModulesViewModel();
            var studentName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(studentName);
            var courseId = (int)user.CourseId;
            student.StudentCourse = await _context.Course.Include(a => a.ApplicationUser).Include(u => u.Modules).FirstOrDefaultAsync(u => u.Id == courseId);
    
            student.Student = user;
       

            return View(student);
        }

        //Get:Course students. Lists all students in a course for Student View
        [Authorize(Roles = "Teacher, Student")]
        public async Task<IActionResult> Student()
        {
            var student = new StudentViewModel();
            var studentName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(studentName);
            ViewBag.StudentName = user.Name;
            var courseId = (int)user.CourseId;




            student.StudentCourse = await _context.Course.Include(x => x.Documents).Include(a => a.ApplicationUser)
                .Include(u => u.Modules).ThenInclude(v => v.LmsActivities).ThenInclude(x => x.Documents)
                .Include(u => u.Modules)
                .ThenInclude(x => x.Documents).FirstOrDefaultAsync(u => u.Id == courseId);
            student.StudentDocuments = new List<Document>();
            foreach (var item in student.StudentCourse.Documents)
            {
                student.StudentDocuments.Add(item);
            }
            
            
            foreach (var module in student.StudentCourse.Modules)
            {
                foreach (var moduldocs in module.Documents)
                {
                    student.StudentDocuments.Add(moduldocs);
                }

            }
            foreach (var module in student.StudentCourse.Modules)
            {
                foreach (var activity in module.LmsActivities)
                {
                    foreach (var activityDocs in activity.Documents)
                    {
                        student.StudentDocuments.Add(activityDocs);
                    }

                }

            }

            
            student.CourseDocument = await _context.Document.Include(u => u.Course).FirstOrDefaultAsync(u => u.Id == courseId);
            student.ModuleDocument
                = await _context.Document.Include(u => u.Course).ThenInclude(u => u.Modules).FirstOrDefaultAsync(u => u.Id == courseId);

            student.Student = user;
      



            return View(student);
        }

        // GET: Courses/Details/5
        [Authorize(Roles = "Teacher, Student")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Teacher")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EndDate,StartDate,Description,Name")] Course course)
        {
            if (course.StartDate.Date <= course.EndDate.Date)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(course);
                    await _context.SaveChangesAsync();
                    TempData["SuccessText"] = $"Kurs: {course.Name} skapades Ok!";
                    // Skapar mapp i filsystemet
                    DirectoryHandler.CreateNewCourseFolders(course.Id.ToString());
                    
                    return RedirectToAction(nameof(Index));
                }
                TempData["FailText"] = "Något gick fel vid skapandet av modulen. Försök igen";
                return View(course);
            }
            else
            {
                TempData["FailText"] = "Startdatum kan inte ligga senare än slutdatum!";
                return View(course);
            }
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EndDate,StartDate,Description,Name")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (course.StartDate.Date <= course.EndDate.Date)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(course);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CourseExists(course.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    TempData["SuccessText"] = $"Kurs: {course.Name} uppdaterad Ok!";
                    return RedirectToAction(nameof(Index));
                }
                TempData["FailText"] = $"Något gick fel. Kurs: {course.Name} uppdaterades inte!";
                return View(course);
            }
            else
            {
                TempData["FailText"] = "Startdatum kan inte ligga senare än slutdatum!";
                return View(course);
            }
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [Authorize(Roles = "Teacher")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            TempData["SuccessText"] = $"Kurs: {course.Name} raderad Ok!";
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }


    }
}

