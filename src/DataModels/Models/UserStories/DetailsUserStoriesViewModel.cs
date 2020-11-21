using Ganss.XSS;
using System.Collections.Generic;

namespace DataModels.Models.UserStories
{
    public class DetailsUserStoriesViewModel
    {
        public UserStoryViewModel ViewModel { get; set; }

        public ICollection<BacklogPriorityDropDownModel> PrioritiesDropDown { get; set; }

        public ICollection<UsersInProjectDropDown> UsersInProjectDropDown { get; set; }
    }
}
