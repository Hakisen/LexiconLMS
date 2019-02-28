using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        //public string TelNr { get; set; }

        public Course Course { get; set; }
        public int CourseId { get; set; }
    }
}
