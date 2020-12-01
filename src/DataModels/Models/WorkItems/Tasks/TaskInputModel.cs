using Ganss.XSS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.Models.WorkItems.Tasks
{
    public class TaskInputModel
    {
        private readonly HtmlSanitizer sanitizer;

        public TaskInputModel()
        {
            this.sanitizer = new HtmlSanitizer();
        }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Range(0, int.MaxValue)]
        public int IdForProject { get; set; }

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
        [Display(Name = "Assigned To")]
        public int? UserId { get; set; }

        public ICollection<AssignToDropdown> UsersDropdown { get; set; }
    }
}
