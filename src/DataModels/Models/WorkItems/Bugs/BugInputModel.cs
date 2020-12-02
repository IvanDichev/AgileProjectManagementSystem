using DataModels.Models.Severity;
using Ganss.XSS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.Models.WorkItems.Bugs
{
    public class BugInputModel
    {
        private readonly HtmlSanitizer sanitizer;
        public BugInputModel()
        {
            this.sanitizer = new HtmlSanitizer();
        }

        public int IdForProject { get; set; }

        [MaxLength(200)]
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => this.sanitizer.Sanitize(this.Description);

        [MaxLength(3000)]
        [Display(Name = "Acceptance Criteria")]
        public string AcceptanceCriteria { get; set; }

        public string SanitizedAcceptanceCriteria => this.sanitizer.Sanitize(this.AcceptanceCriteria);

        [Range(0, int.MaxValue)]
        [Display(Name = "User Story")]
        public int UserStoryId { get; set; }

        public ICollection<UserStoryDropDownModel> UserStoryDropDown { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Severity")]
        public int SeverityId { get; set; }

        public ICollection<SeverityDropDownModel> SeverityDropDown { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Assign To")]
        public int? UserId { get; set; }

        public ICollection<AssignToDropdown> UsersDropdown { get; set; }
    }
}
