using Ganss.XSS;
using System.ComponentModel.DataAnnotations;

namespace DataModels.Models.UserStories
{
    public class UserStoryViewModel
    {
        private readonly HtmlSanitizer htmlSanitizer;
        
        public UserStoryViewModel()
        {
            this.htmlSanitizer = new HtmlSanitizer();
        }

        public int Id { get; set; }

        [MaxLength(200)]
        [Required]
        public string Title { get; set; }

        [Range(0, ushort.MaxValue)]
        public ushort StoryPoints { get; set; }

        public string BacklogPriorityPriority { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => this.htmlSanitizer.Sanitize(this.Description);

        [MaxLength(2000)]
        public string AcceptanceCriteria { get; set; }

        public string SanitizedAcceptanceCriteria => this.htmlSanitizer.Sanitize(this.AcceptanceCriteria);
    }
}
