using DataModels.Models.Comments;
using System;

namespace DataModels.Models.WorkItems
{
    public class WorkItemUpdateModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public ushort? StoryPoints { get; set; }

        public int BacklogPriorityid { get; set; }

        public string Description { get; set; }

        public string AcceptanceCriteria { get; set; }

        public int ProjectId { get; set; }

        public DateTime ModifedOn { get; set; }

        public CommentInputModel Comment { get; set; }
    }
}
