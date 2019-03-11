using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class UsersViewModel
    {
        public List<ApplicationUser> ApplicationUsers { get; set; }
        public Course StudentCourse { get; set; }
        public ApplicationUser Student { get; set; }
    }
}
