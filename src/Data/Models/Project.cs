using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class Project : BaseModel
    {
        public Project()
        {
            //this.Users = new HashSet<UserProject>();
            this.Tickets = new HashSet<Ticket>();
            this.Sprints = new HashSet<Sprint>();
        }

        public DateTime DueDate { get; set; }

        public string Status { get; set; }

        //public virtual ICollection<UserProject> Users { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public ICollection<Sprint> Sprints { get; set; }

        public string UserStoryId { get; set; }
        public virtual UserStory UserStories { get; set; }

        public Team Team { get; set; }
    }
}
