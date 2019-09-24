using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.API.Models
{
    public class ProjectResponse : Project
    {
        public int UserId { get; set; }

        public int TaskCount { get; set; }

        public User User { get; set; }
    }
}
