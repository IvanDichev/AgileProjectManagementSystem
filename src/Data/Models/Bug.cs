using Data.Models.Base;
using Data.Models.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Bug : BaseEntity<int>
    {
        public Bug()
        {
            this.Mockups = new HashSet<Mockup>();
            this.Comments = new HashSet<Comment>();
        }

        [MaxLength(200)]
        public string Title { get; set; }

        public ushort? StoryPoints { get; set; }

        public string Description { get; set; }

        [MaxLength(3000)]
        public string AcceptanceCriteria { get; set; }

        public int UserStoryId { get; set; }

        public virtual UserStory UserStory { get; set; }

        public int SeverityId { get; set; }

        public virtual Severity Severity { get; set; }

        public int? UserId { get; set; }

        public User AssignedTo { get; set; }

        public ICollection<Mockup> Mockups { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
