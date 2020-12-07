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

        //public int ProjectId { get; set; }

        //public virtual Project Project { get; set; }

        public int? SprintId { get; set; }

        public virtual Sprint Sprint { get; set; }
    }
}
