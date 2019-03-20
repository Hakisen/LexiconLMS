using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class StudentModulesViewModel
    {
        [Display(Name = "Elevkurs")]
        public Course StudentCourse { get; set; }

        [Display(Name = "Elev")]
        public ApplicationUser Student { get; set; }

    }
}
