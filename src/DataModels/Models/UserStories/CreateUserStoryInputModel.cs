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
        [Display(Name = "Story Points")]
        public ushort? StoryPoints { get; set; }

        [Display(Name = "Priority")]
        public int BacklogPriorityid { get; set; }

        public string Description { get; set; }

        [MaxLength(2000)]
        [Display(Name = "Acceptance Criteria")]
        public string AcceptanceCriteria { get; set; }

        public int ProjectId { get; set; }

        public ICollection<BacklogPriorityDropDownModel> PrioritiesDropDown { get; set; }
    }
}
