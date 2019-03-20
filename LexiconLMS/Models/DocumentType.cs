using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    //Not used(?)
    public class DocumentType
    {
        public int Id { get; set; }
        public Type LmsType { get; set; }  //Course, Module, LmsActivity or LmsTask
    }
}
