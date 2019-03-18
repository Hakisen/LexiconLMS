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
        // GET: Students per course
        [Authorize(Roles = "Teacher, Student")]
        public IActionResult StudentsPerCourse(int? id)
        {
            var studentspercourse = _context.Course.Include(a => a.ApplicationUser).
                 FirstOrDefault(u => u.Id == id);
            return View(studentspercourse);
        }
        // GET: Modules per course
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

        //Get:Course students and student and student
        [Authorize(Roles = "Teacher, Student")]
        public async Task<IActionResult> Student()
        {
            var student = new StudentViewModel();
            var studentName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(studentName);
            var courseId = (int)user.CourseId;


            student.StudentCourse = await _context.Course.Include(a => a.ApplicationUser).FirstOrDefaultAsync(u => u.Id == courseId);
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
                return RedirectToAction(nameof(Index));
            }
            return View(course);
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
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }


    }
}

