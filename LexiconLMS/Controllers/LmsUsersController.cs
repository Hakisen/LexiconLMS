using LexiconLMS.Data;
using LexiconLMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Controllers
{
    public class LmsUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
  //      private readonly RoleManager<ApplicationUser> roleManager;
        public virtual System.Linq.IQueryable<ApplicationUser> Users { get; }
    //    public virtual System.Linq.IQueryable<ApplicationUser> Roles { get; }


        public LmsUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
    //        this.roleManager = roleManager;
        }

        // GET: Users
        public IActionResult Index()
        {
            // userManager.GetUsersInRoleAsync("Teacher");
            var lmsusers = _context.Users
                 .Include(c => c.Course);
               
           
                   return View(lmsusers);
        }
        // GET: Students
        public async Task<IActionResult> Students()
        {
            var students = await userManager.GetUsersInRoleAsync("Student");
    

            return View(students);
        }

        // GET: LmsUsers/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lmsuser = _context.Users
                .Include(c => c.Course)
                .FirstOrDefault(m => m.Id == id);
                
            if (lmsuser == null)
            {
              return NotFound();
            }

            return View(lmsuser);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var LmsUser = await _context.Users.FindAsync(id);
            if (LmsUser == null)
            {
                return NotFound();
            }
            return View(LmsUser);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,PhoneNumber,Email,Name")] ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                 var user =  await _context.Users.FindAsync(applicationUser.Id);
                    user.Email = applicationUser.Email;
                    user.PhoneNumber = applicationUser.PhoneNumber;
                    user.Name = applicationUser.Name;
                 var result  = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var x = 1;
                    }
                // _context.Update<ApplicationUser>(applicationUser);
                 // await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is ApplicationUser)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach (var property in proposedValues.Properties)
                            {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];
                                if (databaseValues[property] != databaseValues[property]) databaseValues[property] = proposedValues[property];

                                // TODO: decide which value should be written to database
                                // proposedValues[property] = <value to be saved>;
                            }

                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }







                    if (!UserExists(applicationUser.Id))
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
            return View(applicationUser);
        }



        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }


        //private readonly UserManager<ApplicationUser> _userManager;
        //public LmsUsersController(
        //    UserManager<ApplicationUser> userManager)
        //{
        //    _userManager = userManager;

        //}

        //public  Task<IActionResult> Index()
        //{

        //    return View(Users);
        //}


    }
}
