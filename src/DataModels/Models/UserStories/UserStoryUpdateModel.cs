﻿using System;

namespace DataModels.Models.UserStories
{
    public class UserStoryUpdateModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public ushort? StoryPoints { get; set; }

        public int BacklogPriorityid { get; set; }

        public string Description { get; set; }

        public string AcceptanceCriteria { get; set; }

        public int ProjectId { get; set; }

        public DateTime AddedOn { get; set; }

        public DateTime ModifedOn { get; set; }
    }
}
