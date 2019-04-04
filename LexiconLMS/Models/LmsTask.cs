using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class LmsTask
    {

        public int Id { get; set; }
        public string TeacherFeedback { get; set; }
   

        [DataType(DataType.Date)]
        [Display(Name = "Färdig")]
        public System.DateTime ReadyDate { get; set; }

        
        [Display(Name = "Svar")]
        public string StudentAnswer { get; set; }
        [Display(Name = "Elevens kommentar")]
        public string StudentComment{ get; set; } //CourseName
                                         //public string ApplicationUserId { get; set; }

        //public  string ApplicationUserId{ get; set; }//FK
        //nav prop
        public int ReadyStateId { get; set; }

        //[Display(Name = "Aktivitetstyp")]
        public ReadyState ReadyState { get; set; }

        public int LmsActivityId { get; set; } //FK
        public LmsActivity LmsActivity { get; set; }
        public ICollection<Document> Documents { get; set; }
        //nav prop
        public string ApplicationUserId { get; set; } //FK
        public ApplicationUser ApplicationUser { get; set; }

    }
}
