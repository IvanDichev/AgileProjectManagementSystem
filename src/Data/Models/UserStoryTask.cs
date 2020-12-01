using Data.Models.Base;
using Data.Models.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class UserStoryTask : BaseEntity<int>
    {
        public UserStoryTask()
        {
            this.Comments = new HashSet<Comment>();
        }

        [MaxLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [MaxLength(3000)]
        public string AcceptanceCriteria { get; set; }

        public int UserStoryId { get; set; }

        public virtual UserStory UserStory { get; set; }

        public int? UserId { get; set; }

        public User AssignedTo { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
