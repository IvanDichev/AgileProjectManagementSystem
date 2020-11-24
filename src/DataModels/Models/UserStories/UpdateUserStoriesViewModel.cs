using DataModels.Models.Comments;
using System.Collections.Generic;

namespace DataModels.Models.UserStories
{
    public class UpdateUserStoriesViewModel
    {
        public UserStoryViewModel ViewModel { get; set; }

        public CommentInputModel Comment { get; set; }

        public ICollection<BacklogPriorityDropDownModel> PrioritiesDropDown { get; set; }
    }
}
