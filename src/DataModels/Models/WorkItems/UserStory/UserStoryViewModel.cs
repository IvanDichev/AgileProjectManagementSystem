using DataModels.Models.Comments;
using DataModels.Models.Mockups.Dtos;
using Ganss.XSS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.Models.WorkItems.UserStory
{
    public class UserStoryViewModel
    {
        private readonly HtmlSanitizer htmlSanitizer;

        public UserStoryViewModel()
        {
            this.htmlSanitizer = new HtmlSanitizer();
        }

        public int Id { get; set; }

        public int? SprintId { get; set; }

        [MaxLength(200)]
        [Required]
        public string Title { get; set; }

        [Range(0, ushort.MaxValue)]
        public ushort StoryPoints { get; set; }

        public string BacklogPriorityPriority { get; set; }

        public string BacklogPriorityId { get; set; }

        public string Description { get; set; }

        public string MockupsMockUpPath { get; set; }

        public string SanitizedDescription => this.htmlSanitizer.Sanitize(this.Description);

        [MaxLength(3000)]
        public string AcceptanceCriteria { get; set; }

        public string SanitizedAcceptanceCriteria => this.htmlSanitizer.Sanitize(this.AcceptanceCriteria);

        public ICollection<CommentViewModel> Comments { get; set; }

        public ICollection<MockupDto> Mockups { get; set; }
    }
}
