﻿using Data.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class UserStory : BaseEntity<int>
    {
        public UserStory()
        {
            this.Mockups = new HashSet<Mockup>();
            this.Comments = new HashSet<Comment>();
        }

        [MaxLength(200)]
        public string Title { get; set; }

        public int IdForProject { get; set; }

        public ushort? StoryPoints { get; set; }
        
        public int BacklogPriorityId { get; set; }

        public virtual BacklogPriority BacklogPriority { get; set; }

        public string Description { get; set; }
        
        [MaxLength(3000)]
        public string AcceptanceCriteria { get; set; }

        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public ICollection<Mockup> Mockups { get; set; }

        public ICollection<Comment> Comments { get; set; }
        }
}