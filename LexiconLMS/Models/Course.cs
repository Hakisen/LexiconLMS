using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class Course
    {
        public int Id { get; set; }
        public System.DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public string Name { get; set; } //CourseName

        //public  string ApplicationUserId{ get; set; }//FK
        //nav prop
        public ICollection<ApplicationUser> ApplicationUser { get; set; }
        public ICollection<Module> Modules { get; set; }


    }
}
