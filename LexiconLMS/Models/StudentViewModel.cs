using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class StudentViewModel
    {
       // public List<ApplicationUser> ApplicationUsers { get; set; }
       [Display(Name = "Elevkurs")]
        public Course StudentCourse { get; set; }

        [Display(Name = "Elev")]
        public ApplicationUser Student { get; set; }

    }
}