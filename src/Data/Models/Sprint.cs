using Data.Models.Base;
using Shared.Enums;
using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class Sprint : BaseEntity<int>
    {
        public Sprint()
        {
            this.WorkItems = new HashSet<WorkItem>();
        }

        public DateTime DueDate { get; set; }

        public SprintStatus Status { get; set; }

        public virtual ICollection<WorkItem> WorkItems { get; set; }

    }
}