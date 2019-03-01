using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class LmsActivity
    {
        public int Id { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public string Name { get; set; } //
                                         //FK
        public int ActivityTypeId { get; set; }
        public ActivityType ActivityType { get; set; }
        //Nav prop
        public int ModuleId { get; set; }
        public Module Module { get; set; }


    }
}
