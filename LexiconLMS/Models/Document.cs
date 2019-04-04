using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class Document
    {
        public int Id { get; set; }  //DokumantId

        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Display(Name = "Filnamn")]
        public string OwnerFileName { get; set; }

        [Display(Name = "Filsökväg")]
        public string StoredFilePath { get; set; }

        [Display(Name = "Filstorlek")]
        public long Length { get; set; }

        [Display(Name = "Filtyp")]
        public string ContentType { get; set; }

        [NotMapped]
        public IFormFile MyUploadedFile { set; get; }

        [DataType(DataType.Date)]
        [Display(Name = "Dead-line")]
        public DateTime DueDate { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Skapad datum")]
        public System.DateTime CreatedDate { get; set; }

        public string Path { get; set; }
        public string MimeType { get; set; }
        public int? LmsTaskId { get; set; } //FK
        public LmsTask LmsTask { get; set; }
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
