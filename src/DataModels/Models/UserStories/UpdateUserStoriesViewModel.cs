using Ganss.XSS;
using System.Collections.Generic;

namespace DataModels.Models.UserStories
{
    public class UpdateUserStoriesViewModel
    {
        public UserStoryViewModel ViewModel { get; set; }

        public ICollection<BacklogPriorityDropDownModel> PrioritiesDropDown { get; set; }
    }
}
