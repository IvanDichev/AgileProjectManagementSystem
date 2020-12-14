using Data.Models.Base;

namespace Data.Models
{
    public class BurndownData : BaseEntity<int>
    {
        public int DayNo { get; set; }
        
        public int TotalTasks { get; set; }
        
        public int FinishedTasks { get; set; }
        
        public int SprintId { get; set; }

        public virtual Sprint Sprint { get; set; }

        //  tasksRemaining = totalTasks - finishedTasks
        //  scopeChange = totaltasksFromdayOne - totalTasksNow

        //  id  DayNo   sprintId    totalTasks  finishedTasks
        //  1   1       1           10          0
        //  2   2       1           10          1
        //  3   3       1           10          3
        //  4   4       1           11          4
        //  5   5       1           11          6
        //  6   6       1           11          9
        //  7   7       1           11          11
        //  8   1       2           7           1 sch 0 - tr 6
        //  9   2       2           8           1
    }
}
