using Ganss.XSS;

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

        public string Title { get; set; }

        public int StoryPoints { get; set; }

        public string BacklogPriorityPriority { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => this.htmlSanitizer.Sanitize(this.Description);

        public string AcceptanceCriteria { get; set; }

        public string SanitizedAcceptanceCriteria => this.htmlSanitizer.Sanitize(this.AcceptanceCriteria);
    }
}
