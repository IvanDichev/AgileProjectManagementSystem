using Data.Models.Base;
using System.Collections.Generic;

namespace Data.Models
{
    public class KanbanBoard : BaseEntity<int>
    {
        public KanbanBoard()
        {
            this.KanbanBoardColumns = new HashSet<KanbanBoardColumn>();
        }

        public ICollection<KanbanBoardColumn> KanbanBoardColumns { get; set; }

        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public int? SprintId { get; set; }

        public virtual Sprint Sprint { get; set; }
    }
}
