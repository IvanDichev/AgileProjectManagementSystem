using Ganss.XSS;
using System;

namespace DataModels.Models.Comments
{
    public class CommentViewModel
    {
        private HtmlSanitizer sanitizer;
        public CommentViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
        }

        public int Id { get; set; }

        public DateTime AddedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => this.sanitizer.Sanitize(this.Description);

        public string AddedByEmail { get; set; }

        public int WorkItemId { get; set; }
    }
}
