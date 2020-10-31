using Data.Models.Base;
using System.Collections.Generic;

namespace Data.Models
{
    public class Mockup : BaseModel<int>
    {
        public Mockup()
        {
            this.MockupAttachments = new HashSet<MockupAttachment>();
        }

        public ICollection<MockupAttachment> MockupAttachments { get; set; }

        public int UserStoryId { get; set; }
        public virtual UserStory UserStory { get; set; }
    }
}