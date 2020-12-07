using DataModels.Models.Comments;
using System;

namespace DataModels.Models.WorkItems.UserStory
{
    public class UserStoryUpdateModel
    {
        public int Id { get; set; }

        public int? SprintId { get; set; }

        public string Title { get; set; }

        public ushort? StoryPoints { get; set; }

        public int BacklogPriorityid { get; set; }

        public string Description { get; set; }

        public string AcceptanceCriteria { get; set; }

        public DateTime ModifedOn { get; set; }

        public CommentInputModel Comment { get; set; }
    }
}
