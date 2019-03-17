using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Tasks.Models
{
    public class ApplicationUser : IdentityUser
    {
        [InverseProperty("Author")]
        public virtual List<Task> Tasks { get; set; }

        [InverseProperty("Assignee")]
        public virtual List<Task> AssignedTasks { get; set; }
    }
}
