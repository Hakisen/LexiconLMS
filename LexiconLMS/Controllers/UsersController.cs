using LexiconLMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Controllers
{
    public class UsersController : Controller
    {
       

        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        
        }
        // GET: Courses
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.ToList());
        }

        private IActionResult View(object p)
        {
            throw new NotImplementedException();
        }
    }
}
