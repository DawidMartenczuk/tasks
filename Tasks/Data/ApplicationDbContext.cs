using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tasks.Models;

namespace Tasks.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tasks.Models.Task> Task { get; set; }
        public DbSet<Tasks.Models.TaskLog> TaskLog { get; set; }
        public DbSet<Tasks.Models.Milestone> Milestone { get; set; }
    }
}
