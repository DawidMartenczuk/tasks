using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tasks.Models
{
    public class TaskLog
    {
        public int Id { get; set; }

        [Required, StringLength(128, MinimumLength = 3)]
        public string Title { get; set; }
        [Required, StringLength(1024, MinimumLength = 3)]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? Updated { get; set; }

        [Display(Name = "Task")]
        public int TaskId { get; set; }
        public Task Task { get; set; }

        [Display(Name = "User")]
        public string UserId { get; set; }
        [ForeignKey("UserId"), Display(Name = "User")]
        public ApplicationUser User { get; set; }
    }
}
