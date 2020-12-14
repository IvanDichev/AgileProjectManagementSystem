using System;
using System.Collections.Generic;

namespace DataModels.Models.Board
{
    public class BurndownViewModel
    {
        public ICollection<string> DaysInSprint { get; set; }
        //public ICollection<int> DaysInSprint { get; set; }

        public ICollection<int> TasksRemaining { get; set; }

        public ICollection<int> ScopeChanges { get; set; }
    }
}
