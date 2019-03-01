using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class Module
    {

        public int Id { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public string Description { get; set; }
        public string Name { get; set; } //
                                          //Nav prop
        public int CourseId { get; set; } //FK
        public Course Course { get; set; }

        public ICollection<LmsActivity> LmsActivities { get; set; }

    }
}
