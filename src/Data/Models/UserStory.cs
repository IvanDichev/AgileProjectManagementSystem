using Shared.Enums;
using System.Collections.Generic;

namespace Data.Models
{
    public class UserStory : BaseModel
    {
        public UserStory()
        {
            this.Tasks = new HashSet<Task>();
            this.Mockups = new HashSet<Mockup>();
            this.Comments = new HashSet<Comment>();
        }

        public string Name { get; set; }

        public int StoryPoints { get; set; }

        public BacklogPriority PriorityLevel { get; set; }

        public string Description { get; set; }

        public string AcceptanceCriteria { get; set; }

        public ICollection<Task> Tasks { get; set; }

        public ICollection<Mockup> Mockups { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}