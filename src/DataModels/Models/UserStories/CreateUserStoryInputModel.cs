using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.Models.UserStories
{
    public class CreateUserStoryInputModel
    {
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }

        [Range(0, ushort.MaxValue)]
        public ushort? StoryPoints { get; set; }

        public string BacklogPriorityPriority { get; set; }
        public string BacklogPriorityid { get; set; }

        public string Description { get; set; }

        [MaxLength(2000)]
        public string AcceptanceCriteria { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public ICollection<string> MockupsMockupAttachmentsAttachment { get; set; }

        public ICollection<string> CommentsDescription { get; set; }
    }
}
