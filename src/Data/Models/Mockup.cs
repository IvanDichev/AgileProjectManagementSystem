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

        public int WorkItemId { get; set; }
        public virtual UserStory WorkItem { get; set; }
    }
}