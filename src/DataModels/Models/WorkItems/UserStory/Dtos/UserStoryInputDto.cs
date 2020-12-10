using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.Models.WorkItems.UserStory.Dtos
{
    public class UserStoryInputDto
    {
        public string Title { get; set; }

        public ushort? StoryPoints { get; set; }

        public int BacklogPriorityid { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription { get; set; }

        public string AcceptanceCriteria { get; set; }

        public string SanitizedAcceptanceCriteria { get; set; }

        public string MockupPath { get; set; }

        public int ProjectId { get; set; }
    }
}
