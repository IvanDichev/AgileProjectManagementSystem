using Shared.Enums;
using System;
using System.Collections.Generic;

namespace Web.Data.Models
{
    public class Sprint
    {
        public Sprint()
        {
            this.SprintBacklog = new HashSet<SprintBacklog>();
        }

        public DateTime DueDate { get; set; }

        public SprintStatus Status { get; set; }

        public virtual ICollection<SprintBacklog> SprintBacklog { get; set; }
    }
}