using Ganss.XSS;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace DataModels.Models.WorkItems.UserStory
{
    public class UserStoryInputModel
    {
        private readonly HtmlSanitizer htmlSanitizer;

        public UserStoryInputModel()
        {
            htmlSanitizer = new HtmlSanitizer();
        }

        [MaxLength(200)]
        [Required]
        public string Title { get; set; }

        [Range(0, ushort.MaxValue)]
        [Display(Name = "Story Points")]
        public ushort? StoryPoints { get; set; }

        [Display(Name = "Priority")]
        public int BacklogPriorityid { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => htmlSanitizer.Sanitize(Description);

        [MaxLength(3000)]
        [Display(Name = "Acceptance Criteria")]
        public string AcceptanceCriteria { get; set; }

        public string SanitizedAcceptanceCriteria => htmlSanitizer.Sanitize(AcceptanceCriteria);

        public IFormFile Mockup { get; set; }

        public int ProjectId { get; set; }

        public ICollection<BacklogPriorityDropDownModel> PrioritiesDropDown { get; set; }
    }
}
