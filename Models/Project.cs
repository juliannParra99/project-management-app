using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementApp.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        // Navigation property for the tasks associated with this project; relation with foreind key into ProjectTask
        //define relation one to many with ProjectTask
        public List<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    }
}

