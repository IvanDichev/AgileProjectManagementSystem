using Data.Models.Base;
using System;

namespace Data.Models
{
    public class BurndownData : BaseEntity<int>
    {
        public DateTime DayOfSprint { get; set; }
        
        public int TotalTasks { get; set; }
        
        public int FinishedTasks { get; set; }
        
        public int SprintId { get; set; }

        public virtual Sprint Sprint { get; set; }
    }
}
