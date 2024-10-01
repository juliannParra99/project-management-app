using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Models;

namespace ProjectManagementApp.Data
{
    public class ApiDbContext : IdentityDbContext
    {

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //define relation one to many
            builder.Entity<Project>()
                .HasMany(p => p.Tasks) //A project has many tasks
                .WithOne() //Each task is related to one project
                .HasForeignKey(t => t.ProjectId); //The foreign key is 'ProjectId' in the ProjectTask entity

        }


    }
}