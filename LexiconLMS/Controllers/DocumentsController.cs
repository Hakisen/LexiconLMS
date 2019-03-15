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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace LexiconLMS.Controllers
{
    public class DocumentsController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment hostingEnvironment;

        public DocumentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment environment)
        {

            _context = context;
            _userManager = userManager;
            hostingEnvironment = environment;
        }


        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Document.Include(d => d.ApplicationUser).Include(d => d.Course).Include(d => d.LmsActivity).Include(d => d.Module);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                .Include(d => d.ApplicationUser)
                .Include(d => d.Course)
                .Include(d => d.LmsActivity)
                .Include(d => d.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id");
            ViewData["LmsActivityId"] = new SelectList(_context.LmsActivity, "Id", "Id");
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Id");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,MyProperty,CreatedDate,Path,MimeType,Id,CourseId,ModuleId,LmsActivityId,ApplicationUserId")] Document document)
        {
            if (ModelState.IsValid)
            {
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", document.ApplicationUserId);
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", document.CourseId);
            ViewData["LmsActivityId"] = new SelectList(_context.LmsActivity, "Id", "Id", document.LmsActivityId);
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Id", document.ModuleId);
            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", document.ApplicationUserId);
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", document.CourseId);
            ViewData["LmsActivityId"] = new SelectList(_context.LmsActivity, "Id", "Id", document.LmsActivityId);
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Id", document.ModuleId);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,MyProperty,CreatedDate,Path,MimeType,Id,CourseId,ModuleId,LmsActivityId,ApplicationUserId")] Document document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.Id))
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
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", document.ApplicationUserId);
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", document.CourseId);
            ViewData["LmsActivityId"] = new SelectList(_context.LmsActivity, "Id", "Id", document.LmsActivityId);
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Id", document.ModuleId);
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                .Include(d => d.ApplicationUser)
                .Include(d => d.Course)
                .Include(d => d.LmsActivity)
                .Include(d => d.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Document.FindAsync(id);
            _context.Document.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return _context.Document.Any(e => e.Id == id);
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourseDocument([Bind("Id,Title,Description,CourseId,ApplicationUserId,CreatedDate")] Document document)
        {

            var course = await _context.Course.FindAsync(document.CourseId);


            if (ModelState.IsValid)
            {
                _context.Add(document);
                await _context.SaveChangesAsync();
                TempData["SuccessText"] = $"Modul: {document.Title} skapades Ok!";
                return RedirectToAction(nameof(CourseDocuments), new { document.CourseId });
            }

            TempData["FailText"] = $"Något gick fel vid skapandet av modulen. Försök igen";
            //ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Name", document.CourseId);
            document.Course = course;
            return View(document);
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> CreateCourseDocument(int courseId)
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
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userId= await _userManager.GetUserIdAsync(user);
            
            Document model = new Document
            {
                CourseId = courseId,
                ApplicationUser = user,
                ApplicationUserId = userId,
                Course = course,
                CreatedDate=DateTime.Today
            };

            return base.View(model);
        }
        [HttpPost]
        public IActionResult CreateCourseDocument1( Document model)
        {
            // do other validations on your model as needed
            if (model.MyUploadedFile.FileName != null)
            {
                var uniqueFileName = GetUniqueFileName(model.MyUploadedFile.FileName);
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                model.MyUploadedFile.CopyTo(new FileStream(filePath, FileMode.Create));

                //to do : Save uniqueFileName  to your db table   
            }
            // to do  : Return something
            return RedirectToAction("Index", "Home");
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        public async Task<IActionResult> CourseDocuments(int? courseId)
        {
            var applicationDbContext = _context.Document.Include(c => c.Course).Where(c => c.Course.Id == courseId);
            //return View("Index", await applicationDbContext.ToListAsync());
            ViewBag.CourseName = _context.Course.Find(courseId).Name;
            ViewBag.CourseId = courseId;
            return View(await applicationDbContext.ToListAsync());
        }


        
        public IActionResult CreateCourseDocument1()
        {

            return View(new Document());
          
        }
    }


}

