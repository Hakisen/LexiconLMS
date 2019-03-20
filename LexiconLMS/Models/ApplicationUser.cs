using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Namn")]
        public string Name { get; set; }

        //public string TelNr { get; set; }
        [Display(Name = "Kurs")]
        public Course Course { get; set; }

        [Display(Name = "KursId")]
        public int? CourseId { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}
