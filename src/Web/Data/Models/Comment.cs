using System;
using Web.Data.Models.Users;

namespace Web.Data.Models
{
    public class Comment : BaseModel
    {
        public string Description { get; set; }

        public DateTime AddedOn { get; set; }

        public DateTime ModifedOn { get; set; }

        public string UserId { get; set; }
        public virtual User AddedBy { get; set; }

        public string UserStoryId { get; set; }
        public virtual UserStory UserStory { get; set; }
    }
}