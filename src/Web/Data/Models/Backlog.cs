using Shared.Enums;
using System.Collections.Generic;

namespace Web.Data.Models
{
    public class Backlog : BaseModel
    {
        public Backlog()
        {
            this.UserStories = new HashSet<UserStorie>();
        }

        public string Name { get; set; }

        public int Effort { get; set; }

        public BacklogPriority PriorityLevel { get; set; }

        public string Description { get; set; }

        public ICollection<UserStorie> UserStories { get; set; }

        public string AcceptanceCriteria { get; set; }
    }
}