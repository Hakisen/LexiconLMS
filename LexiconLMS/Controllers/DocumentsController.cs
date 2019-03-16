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
using Microsoft.Extensions.FileProviders;

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
        public async Task<IActionResult> Create([Bind("Title,DueDate,CreatedDate,Path,MimeType,Id,CourseId,ModuleId,LmsActivityId,ApplicationUserId")] Document document)
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
        public async Task<IActionResult> Edit(int id, [Bind("Title,DueDate,CreatedDate,Path,MimeType,Id,CourseId,ModuleId,LmsActivityId,ApplicationUserId")] Document document)
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
        public async Task<IActionResult> CreateCourseDocument1([Bind("MyUploadedFile,Title,DueDate,CreatedDate,Path,MimeType,Id,CourseId,ModuleId,LmsActivityId,ApplicationUserId,OwnerFileName")]Document document)
        {




            // do other validations on your model as needed
            if (document.MyUploadedFile.FileName != null)
            {

                var uniqueFileName = GetUniqueFileName(document.MyUploadedFile.FileName);
                var documentroot = Path.Combine(hostingEnvironment.WebRootPath,"documents");

                var fullpath = Path.Combine(documentroot, uniqueFileName);


                document.MyUploadedFile.CopyTo(new FileStream(fullpath, FileMode.Create));

                //to do : Save uniqueFileName  to your db table   

                var course = await _context.Course.FindAsync(document.CourseId);
                document.OwnerFileName = document.MyUploadedFile.FileName;
                document.StoredFilePath = uniqueFileName;
                document.ContentType = document.MyUploadedFile.ContentType;
                document.Length = document.MyUploadedFile.Length;

                if (ModelState.IsValid)
                {
                    _context.Add(document);
                    await _context.SaveChangesAsync();
                    TempData["SuccessText"] = $"Modul: {document.Title} skapades Ok!";
                    return RedirectToAction(nameof(CourseDocuments), new { document.CourseId });
                }

                TempData["FailText"] = $"Något gick fel vid skapandet av modulen. Försök igen";
              
                document.Course = course;
            }

          
            return RedirectToAction("CourseDocuments", "Documents");
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        public async Task<IActionResult> CourseDocuments(int? courseId,String LmsType)
        {
            var applicationDbContext = _context.Document.Include(c => c.Course).Where(c => c.Course.Id == courseId);
            if (LmsType == "Course")
            {
               
                //return View("Index", await applicationDbContext.ToListAsync());
                ViewBag.CourseName = _context.Course.Find(courseId).Name;
                ViewBag.CourseId = courseId;
            }
                return View(await applicationDbContext.ToListAsync());
            
        }


        
        public async Task<IActionResult> CreateCourseDocument1(int courseId)
        {
            

            var course = _context.Course.Find(courseId);
     
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userId = await _userManager.GetUserIdAsync(user);

            Document model = new Document
            {
                CourseId = courseId,
                ApplicationUser = user,
                ApplicationUserId = userId,
                Course = course,
                CreatedDate = DateTime.Today
            };

            return View(model);

        }
        private string fileName { get; set; }
        private string uniqueFileName { get; set; }

        //public IActionResult getFile()
        //{
        //    string wwwrootPath = hostingEnvironment.WebRootPath;
        //    fileName = @"Employees.xlsx";
        //    FileInfo file = new FileInfo(Path.Combine(wwwrootPath, fileName));
        //    return downloadFile(wwwrootPath);
        //}
        public FileResult downloadFile(string filePath,string mimetype)
        {
            IFileProvider provider = new PhysicalFileProvider(filePath);
            IFileInfo fileInfo = provider.GetFileInfo(uniqueFileName);
            var readStream = fileInfo.CreateReadStream();
       
            return File(readStream, mimetype, fileName,true);
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> GetDocument(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }
          uniqueFileName = document.StoredFilePath;
            var mimeType = document.ContentType;
            var documentroot = Path.Combine(hostingEnvironment.WebRootPath, "documents");
            FileInfo file = new FileInfo(Path.Combine(documentroot, uniqueFileName));
            fileName = document.OwnerFileName;

            return downloadFile(documentroot, mimeType);
          
          
        }




        public async Task<IActionResult> LmsDocuments(int? Id, String LmsType)
        {
           
            if (LmsType == "Course")
            {
                var applicationDbContext = _context.Document.Include(c => c.Course).Where(c => c.Course.Id == Id);

              
                ViewBag.CourseName = _context.Course.Find(Id).Name;
                ViewBag.CourseId = Id;
                return View(await applicationDbContext.ToListAsync());
            }
            if (LmsType == "Module")
            {
                var applicationDbContext = _context.Document.Include(c => c.Module).Where(c => c.Module.Id == Id);

               
                ViewBag.ModuleName = _context.Module.Find(Id).Name;
                ViewBag.ModuleId = Id;
                return View(await applicationDbContext.ToListAsync());

            }
            if (LmsType == "LmsActivity")
            {
                var applicationDbContext = _context.Document.Include(c => c.Module).Where(c => c.Module.Id == Id);

              
                ViewBag.LmsActivityName = _context.LmsActivity.Find(Id).Name;
                ViewBag.LmsActivityId = Id;
                return View(await applicationDbContext.ToListAsync());

            }
            else
            return View(await _context.Document.ToListAsync());  //Should not happen

        }



        public async Task<IActionResult> CreateLmsDocument(int Id)
        {


            var course = _context.Course.Find(Id);

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userId = await _userManager.GetUserIdAsync(user);

            Document model = new Document
            {
                CourseId = Id,
                ApplicationUser = user,
                ApplicationUserId = userId,
                Course = course,
                CreatedDate = DateTime.Today
            };

            return View(model);

        }







    }


}

