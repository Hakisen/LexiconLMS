﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconLMS.Models
{
    public class ActivityType
    {
        public string Type { get; set; }
        public int Id { get; set; }
        //Nav prop
        public ICollection<LmsActivity> Activities { get; set; }
    }
}
