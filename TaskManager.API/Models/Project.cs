using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Priority { get; set; }
    }
}
