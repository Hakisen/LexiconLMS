using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using LexiconLMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using LexiconLMS.Data;
using System.Runtime.InteropServices;

namespace LexiconLMS.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private int? _courseId;

        public RegisterModel(
         
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            int? CourseId
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
             _context = context;
            _courseId = CourseId;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Phone]
            [Display(Name = "Telefon")]
            public string Phone { get; set; }
            [Required]
            [Display(Name = "Namn")]
            public string Name { get; set; }


            [Display(Name = "Roll")]
            public string Role{ get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Lösenord")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            [Display(Name = "Course")]
            public int CourseId { get; set; }

        }

        public void OnGet(int? CourseId,string returnUrl = null)

        {
            _courseId = CourseId;
            ViewData["RoleName"] = new SelectList(_roleManager.Roles, "Name", "Name");

            ViewData["CourseIdList"] = new SelectList(_context.Course, "Id", "Name");
            ViewData["CourseId"] = _courseId;

            //return View();
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email,PhoneNumber=Input.Phone ,Name=Input.Name };
                var result = await _userManager.CreateAsync(user, Input.Password);
                var resultAddRole = await _userManager.AddToRoleAsync(user, Input.Role);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    if (_courseId != null)
                        user.CourseId = Input.CourseId;
                    else user.CourseId = _courseId;
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                 //Stefan  await _signInManager.SignInAsync(user, isPersistent: false);
             

                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
