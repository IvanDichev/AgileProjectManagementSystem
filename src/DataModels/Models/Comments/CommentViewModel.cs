using System;

namespace DataModels.Models.Comments
{
    public class CommentViewModel
    {
        public DateTime AddedOn { get; set; }
        public DateTime? ModifedOn { get; set; }
        public string Description { get; set; }
        public string AddedByEmail { get; set; }
        public int UserStoryId { get; set; }
    }
}
