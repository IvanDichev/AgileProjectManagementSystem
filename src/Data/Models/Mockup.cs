using Data.Models.Base;
using System.Collections.Generic;

namespace Data.Models
{
    public class Mockup : BaseEntity<int>
    {
        public Mockup()
        {
            this.MockupAttachments = new HashSet<MockupAttachment>();
        }

        public ICollection<MockupAttachment> MockupAttachments { get; set; }

        public int UserStoryId { get; set; }
        public virtual WorkItem UserStory { get; set; }
    }
}