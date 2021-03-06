﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class Module
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Slut")]
        //[DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime EndDate { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Start")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "Namn")]
        public string Name { get; set; } //
                                          //Nav prop
        public int CourseId { get; set; } //FK
        public Course Course { get; set; }

        public ICollection<LmsActivity> LmsActivities { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}
