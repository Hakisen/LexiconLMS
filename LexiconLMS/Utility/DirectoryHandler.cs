using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace LexiconLMS.Utility
{
    public static class DirectoryHandler
    {
        //private readonly IHostingEnvironment _hostingEnvironment;

        public static void CreateNewCourseFolders(string name)
        {
          //   Root foldern
          string applicationPath = Directory.GetCurrentDirectory();
           // string path =@"C:\Users\LexTottedq\Source\Repos\Hakisen\LexiconLMS\LexiconLMS\LMSDocument\";
           // Root folder + LMSDocument folder + önskad foldernamn
            string newMap = applicationPath + @"\LMSDocument\"+"Kurs"+name;

            DirectoryInfo di = new DirectoryInfo(newMap);
           // skapar kursfoldern           
            di.Create();
            // Skapar foldrar under kursfoldern
            DirectoryInfo d2 = new DirectoryInfo(newMap);
            DirectoryInfo s = d2.CreateSubdirectory("Student");
            DirectoryInfo s2 = d2.CreateSubdirectory("Module");
            DirectoryInfo s3 = d2.CreateSubdirectory("Activity");
        }
        public static string GetStudentDirectoryString(string course)
        {
            string applicationPath = Directory.GetCurrentDirectory();
            string newMap = applicationPath + @"\LMSDocument\" + course +@"\Student";
            return newMap;
        }
        public static string GetModuleDirectoryString(string course)
        {
            string applicationPath = Directory.GetCurrentDirectory();
            string newMap = applicationPath + @"\LMSDocument\" + course + @"\Module";
            return newMap;
        }
        public static string GetActivityDirectoryString(string course)
        {
            string applicationPath = Directory.GetCurrentDirectory();
            string newMap = applicationPath + @"\LMSDocument\" + course + @"\Activity";
            return newMap;
        }
    }
    
}
