using DataModels.Models.Comments;
using DataModels.Models.Sprints;
using System.Collections.Generic;

namespace DataModels.Models.WorkItems.UserStory
{
    public class UserStoryUpdateViewModel
    {
        public UserStoryViewModel ViewModel { get; set; }

        public CommentInputModel Comment { get; set; }

        public ICollection<BacklogPriorityDropDownModel> PrioritiesDropDown { get; set; }

        public ICollection<SprintDropDownModel> SprintDropDownModel { get; set; }
    }
}
