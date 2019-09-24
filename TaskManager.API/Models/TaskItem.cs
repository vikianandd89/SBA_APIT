﻿using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Priority { get; set; }

        public int? ParentTaskId { get; set; }

        public bool EndTask { get; set; }

        public Project Project { get; set; }
    }
}
