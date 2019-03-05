using LexiconLMS.Data;
using LexiconLMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Controllers
{
    public class LmsUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public virtual System.Linq.IQueryable<ApplicationUser> Users { get; }
       



        private readonly UserManager<ApplicationUser> _userManager;
        public LmsUsersController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

        }
        
        public  Task<IActionResult> Index()
        {
            
            return View(Users);
        }

     
    }
}
