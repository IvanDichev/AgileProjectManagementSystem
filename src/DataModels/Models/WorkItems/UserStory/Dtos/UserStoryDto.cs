﻿using DataModels.Models.Comments;
using DataModels.Models.Mockups.Dtos;
using System.Collections.Generic;

namespace DataModels.Models.WorkItems.UserStory.Dtos
{
    public class UserStoryDto
    {
        public int Id { get; set; }

        public int? SprintId { get; set; }

        public int IdForProject { get; set; }

        public string Title { get; set; }

        public int StoryPoints { get; set; }

        public string BacklogPriorityid { get; set; }

        public string BacklogPriorityPriority { get; set; }

        public string Description { get; set; }

        public string AcceptanceCriteria { get; set; }

        public ICollection<MockupDto> Mockups { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }
    }
}
