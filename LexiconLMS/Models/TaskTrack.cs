using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class TaskTrack
    {


        // public List<ApplicationUser> ApplicationUsers { get; set; }
        [Display(Name = "Elevkurs")]
        public List<Course> Courses { get; set; }
        public List<Module> Modules { get; set; }
        public List<LmsTask> StudentTasks { get; set; }

        [Display(Name = "Elev")]
        public ApplicationUser Student { get; set; }
        public List<Document> TaskDocuments { get; set; }
    
        public Document ActivityDocument { get; set; }
    

    }
}

