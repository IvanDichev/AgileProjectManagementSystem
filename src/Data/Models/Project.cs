using Data.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Project : BaseModel<int>
    {
        public Project()
        {
            this.Tickets = new HashSet<Ticket>();
            this.Sprints = new HashSet<Sprint>();
        }

        [MaxLength(25)]
        public string Name { get; set; }

        [MaxLength(350)]
        public string Description { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public ICollection<Sprint> Sprints { get; set; }

        public int? UserStoryId { get; set; }
        public virtual UserStory UserStories { get; set; }

        public int? TeamId { get; set; }
        public Team Team { get; set; }
    }
}
