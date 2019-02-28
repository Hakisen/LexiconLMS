using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class ActivityType
    {
        int Id;
        string Type;
        int ActivityId; //FK
        //Nav prop
        ICollection<Activity> Activities;
    }
}
