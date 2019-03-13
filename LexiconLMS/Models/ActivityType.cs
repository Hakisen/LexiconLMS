using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class ActivityType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        //Nav prop
        public ICollection<LmsActivity> LmsActivities { get; set; }
    }
}
