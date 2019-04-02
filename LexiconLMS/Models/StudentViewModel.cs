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
        public List<Module> StudentModules { get; set; }

        [Display(Name = "Elev")]
        public ApplicationUser Student { get; set; }
        public List<Document> StudentDocuments { get; set; }
        public Document CourseDocument { get; set; }
        public Document ModuleDocument { get; set; }
        public Document ActivityDocument { get; set; }
        public Document OwnDocument { get; set; }

    }
}