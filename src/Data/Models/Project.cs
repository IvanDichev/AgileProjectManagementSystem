using Data.Models.Base;
using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class Project : BaseModel<int>
    {
        public Project()
        {
            this.Tickets = new HashSet<Ticket>();
            this.Sprints = new HashSet<Sprint>();
        }

        public DateTime DueDate { get; set; }

        public string Status { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public ICollection<Sprint> Sprints { get; set; }

        public int? UserStoryId { get; set; }
        public virtual UserStory UserStories { get; set; }

        public Team Team { get; set; }
    }
}
