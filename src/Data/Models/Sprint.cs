using Shared.Enums;
using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class Sprint : BaseModel
    {
        public Sprint()
        {
            //this.SprintBacklog = new HashSet<SprintBacklog>();
            this.UserStories = new HashSet<UserStory>();
        }

        public DateTime DueDate { get; set; }

        public SprintStatus Status { get; set; }

        //public virtual ICollection<SprintBacklog> SprintBacklog { get; set; }
        public virtual ICollection<UserStory> UserStories { get; set; }

    }
}