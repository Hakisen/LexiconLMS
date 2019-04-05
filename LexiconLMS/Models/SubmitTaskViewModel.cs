using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class SubmitTaskViewModel
    {
        // public List<ApplicationUser> ApplicationUsers { get; set; }
        [Display(Name = "Elev kurs")]
        public Course StudentCourse { get; set; }
        public Module StudentModule { get; set; }

    
        public List<ApplicationUser> Students { get; set; }
        public LmsActivity  LmsActivity { get; set; }
  
    }
}
