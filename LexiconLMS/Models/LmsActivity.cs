using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class LmsActivity
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Slut")]
        public DateTime EndDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Display(Name = "Namn")]
        public string Name { get; set; } //
                                         //FK
        public int ActivityTypeId { get; set; }

        //[Display(Name = "Aktivitetstyp")]
        public ActivityType ActivityType { get; set; }
        //Nav prop
        public int ModuleId { get; set; }
        public Module Module { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<LmsTask> LmsTasks { get; set; }
    }
}
