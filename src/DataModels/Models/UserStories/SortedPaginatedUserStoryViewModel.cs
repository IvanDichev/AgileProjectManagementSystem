using DataModels.Models.Sorting;
using System.Collections.Generic;

namespace DataModels.Models.UserStories
{
    public class SortedPaginatedUserStoryViewModel
    {
        public ICollection<UserStoryViewModel> UserStoryViewModel { get; set; }

        public SortingFilter SortingFilter { get; set; }
    }
}
