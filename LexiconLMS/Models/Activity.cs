using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class Activity
    {
        int Id;
        DateTime EndDate;
        DateTime StartDate;
        string Description;
        string Name; //
                     //FK
        int ActivityTypeId;
        int ModuleId;
        //Nav prop
        ActivityType ActivityType;
        Module Module;


    }
}
