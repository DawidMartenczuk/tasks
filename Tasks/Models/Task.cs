using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tasks.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required, StringLength(128, MinimumLength = 3)]
        public string Title { get; set; }
        [Required, StringLength(16384, MinimumLength = 16)]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? Updated { get; set; }

        [Display(Name = "Author")]
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        [Display(Name = "Assignee")]
        public string AssigneeId { get; set; }
        public ApplicationUser Assignee { get; set; }

        [Display(Name = "Milestone")]
        public int? MilestoneId { get; set; }
        public Milestone Milestone { get; set; }

        public bool Closed { get; set; }

        public IList<TaskLog> Logs { get; set; }
    }
}
