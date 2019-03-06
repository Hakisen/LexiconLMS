using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class Course
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Slut")]
        public System.DateTime EndDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "Kursnamn")]
        public string Name { get; set; } //CourseName
        //public string ApplicationUserId { get; set; }

        //public  string ApplicationUserId{ get; set; }//FK
        //nav prop
        public ICollection<ApplicationUser> ApplicationUser { get; set; }
        public ICollection<Module> Modules { get; set; }


    }
}
