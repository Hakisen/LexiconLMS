using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class ActivityType
    {
        public int Id { get; set; }

        [Display(Name = "Aktivitetstyp")]
        public string Type { get; set; }

        //Nav prop
        public ICollection<LmsActivity> LmsActivities { get; set; }
    }
}
