using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LexiconLMS.Models
{
    public class ReadyState
    {
        public int Id { get; set; }

        [Display(Name = "Färdigstatus")]
        public string Type { get; set; }

        //Nav prop
        public ICollection<LmsTask> LmsTask { get; set; }
    }
}