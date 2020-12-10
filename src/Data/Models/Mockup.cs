using Data.Models.Base;
using System.Collections.Generic;

namespace Data.Models
{
    public class Mockup : BaseEntity<int>
    {
        public string MockUpPath { get; set; }

        public int WorkItemId { get; set; }

        public virtual UserStory UserStory { get; set; }
    }
}