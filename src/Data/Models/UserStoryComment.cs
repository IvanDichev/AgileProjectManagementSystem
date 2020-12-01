using Data.Models.Base;
using Data.Models.Users;

namespace Data.Models
{
    public class UserStoryComment : BaseEntity<int>
    {
        public string Description { get; set; }

        public int UserId { get; set; }
        public virtual User AddedBy { get; set; }

        public int UserStoryId { get; set; }
        public virtual UserStory UserStory { get; set; }
    }
}