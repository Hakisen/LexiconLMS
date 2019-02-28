using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class Module
    {
       
            int Id;
            DateTime EndDate;
            DateTime StartDate;
            string Description;
            string Name; //
        int CourseId; //FK
        //Nav prop
      ICollection<Activity> Activities;
        Course Course;


        
    }
}
