using Data.Models.Base;
using System.Collections.Generic;

namespace Data.Models
{
    public class KanbanBoardColumn : BaseEntity<int>
    {
        public KanbanBoardColumn()
        {
            this.UserStories = new HashSet<UserStory>();
        }

        public int KanbanBoardColumnOptionId { get; set; }

        public virtual KanbanBoardColumnOption KanbanBoardColumnOption { get; set; }

        public ICollection<UserStory> UserStories { get; set; }

        public int? SprintId { get; set; }

        public virtual Sprint Sprint { get; set; }

        public ICollection<UserStoryTask> Tasks { get; set; }

        public ICollection<Test> Tests { get; set; }

        public ICollection<Bug> Bugs { get; set; }
    }
}
