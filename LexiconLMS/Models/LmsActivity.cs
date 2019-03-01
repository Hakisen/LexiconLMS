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
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [DataType(DataType.DateTime)]
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
