using System.Collections.Generic;
using Data.Models.Base;
using Data.Models.Users;

namespace Data.Models
{
    public class Assignment : BaseEntity<int>
    {
        public Assignment()
        {
            this.SubTasks = new HashSet<SubTask>();
        }

        public string Name { get; set; }

        public int Effort { get; set; }

        public ICollection<SubTask> SubTasks { get; set; }

        public int SprintId { get; set; }
        public virtual Sprint Sprint { get; set; }

        public int UserStoryId { get; set; }
        public virtual WorkItem UserStory { get; set; }

        public int? UserId { get; set; }
        public virtual User CreatedBy { get; set; }
    }
}