using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class Course
    {
        int Id;
        DateTime EndDate;
        DateTime StartDate;
        string Description;
        string Name; //CourseName

        string ApplicationUserId;//FK
        //nav prop
        ApplicationUser ApplicationUser;
        ICollection<Module> Modules;




    }
}
