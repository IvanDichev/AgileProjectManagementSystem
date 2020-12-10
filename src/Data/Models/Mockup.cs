using Data.Models.Base;

namespace Data.Models
{
    public class Mockup : BaseEntity<int>
    {
        public string MockUpPath { get; set; }

        public int UserStoryId { get; set; }

        public virtual UserStory UserStory { get; set; }
    }
}