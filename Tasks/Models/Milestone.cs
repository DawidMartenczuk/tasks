using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tasks.Models
{
    public class Milestone
    {
        public int Id { get; set; }

        [Required, StringLength(128, MinimumLength = 3)]
        public string Title { get; set; }
        [StringLength(1024, MinimumLength = 3)]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime End { get; set; }

        public IList<Task> Tasks { get; set; }
    }
}
