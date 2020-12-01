using DataModels.Models.Comments;
using System.Collections.Generic;

namespace DataModels.Models.WorkItems.UserStory
{
    public class UserStoryUpdateViewModel
    {
        public UserStoryViewModel ViewModel { get; set; }

        public CommentInputModel Comment { get; set; }

        public ICollection<BacklogPriorityDropDownModel> PrioritiesDropDown { get; set; }
    }
}
