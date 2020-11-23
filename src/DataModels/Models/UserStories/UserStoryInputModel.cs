using Ganss.XSS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.Models.UserStories
{
    public class UserStoryInputModel
    {
        private readonly HtmlSanitizer htmlSanitizer;

        public UserStoryInputModel()
        {
            this.htmlSanitizer = new HtmlSanitizer();
        }
        public int Id { get; set; }

        [MaxLength(200)]
        [Required]
        public string Title { get; set; }

        [Range(0, ushort.MaxValue)]
        [Display(Name = "Story Points")]
        public ushort? StoryPoints { get; set; }

        [Display(Name = "Priority")]
        public int BacklogPriorityid { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => this.htmlSanitizer.Sanitize(this.Description);

        [MaxLength(3000)]
        [Display(Name = "Acceptance Criteria")]
        public string AcceptanceCriteria { get; set; }

        public string SanitizedAcceptanceCriteria => this.htmlSanitizer.Sanitize(this.AcceptanceCriteria);

        public int ProjectId { get; set; }

        public ICollection<BacklogPriorityDropDownModel> PrioritiesDropDown { get; set; }
    }
}
