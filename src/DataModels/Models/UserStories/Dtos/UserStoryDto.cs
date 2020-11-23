using DataModels.Models.Comments;
using System.Collections.Generic;

namespace DataModels.Models.UserStories.Dtos
{
    public class UserStoryDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int StoryPoints { get; set; }

        public string BacklogPriorityid { get; set; }

        public string BacklogPriorityPriority { get; set; }

        public string Description { get; set; }

        public string AcceptanceCriteria { get; set; }

        public int TasksCount { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }
    }
}
