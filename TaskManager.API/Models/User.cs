using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int EmployeeId { get; set; } 

        public int? ProjectId { get; set; }

        public int? TaskId { get; set; }

        public virtual Project Project { get; set; }

        public virtual TaskItem Task { get; set; }
    }
}
