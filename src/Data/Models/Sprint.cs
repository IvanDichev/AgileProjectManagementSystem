using Data.Models.Base;
using Shared.Enums;
using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class Sprint : BaseModel<int>
    {
        public Sprint()
        {
            this.UserStories = new HashSet<UserStory>();
        }

        public DateTime DueDate { get; set; }

        public SprintStatus Status { get; set; }

        public virtual ICollection<UserStory> UserStories { get; set; }

    }
}