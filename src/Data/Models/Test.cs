using Data.Models.Base;
using Data.Models.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Test : BaseEntity<int>
    {
        public Test()
        {
            this.Mockups = new HashSet<Mockup>();
            //this.Comments = new HashSet<UserStoryComment>();
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

        public ICollection<Mockup> Mockups { get; set; }

        //public ICollection<UserStoryComment> Comments { get; set; }
    }
}
