using DataModels.Models.Sorting;
using System.Collections.Generic;

namespace DataModels.Models.WorkItems
{
    public class SortedPaginatedUserStoryViewModel
    {
        public ICollection<WorkItemViewModel> WorkItemViewModel { get; set; }

        public SortingFilter SortingFilter { get; set; }
    }
}
