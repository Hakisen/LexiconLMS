using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class Document
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string OwnerFileName { get; set; }
        public IFormFile MyUploadedFile { set; get; }

        public DateTime MyProperty { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Skapad datum")]
        public System.DateTime CreatedDate { get; set; }
        public string Path { get; set; }
        public string MimeType { get; set; }


        public int Id { get; set; }  //DokumantId

        public int? CourseId { get; set; } //FK
        public Course Course { get; set; }
        public int? ModuleId { get; set; } //FK
        public Module Module { get; set; }
        public int? LmsActivityId { get; set; } //FK
        public LmsActivity LmsActivity { get; set; }
        public string ApplicationUserId { get; set; } //FK
        public ApplicationUser ApplicationUser  { get; set; }

    }
}
