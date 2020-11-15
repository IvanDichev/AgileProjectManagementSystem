using Data.Models.Base;
using System.Collections.Generic;

namespace Data.Models
{
    public class UserStory : BaseModel<int>
    {
        public UserStory()
        {
            this.Tasks = new HashSet<Assignment>();
            this.Mockups = new HashSet<Mockup>();
            this.Comments = new HashSet<Comment>();
        }

        public string Title { get; set; }

        public int? StoryPoints { get; set; }
        
        public int BacklogPriorityId { get; set; }

        public virtual BacklogPriority BacklogPriority { get; set; }

        public string Description { get; set; }

        public string AcceptanceCriteria { get; set; }

        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public ICollection<Assignment> Tasks { get; set; }

        public ICollection<Mockup> Mockups { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}