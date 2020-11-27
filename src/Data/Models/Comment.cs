﻿using Data.Models.Base;
using Data.Models.Users;

namespace Data.Models
{
    public class Comment : BaseEntity<int>
    {
        public string Description { get; set; }

        public int UserId { get; set; }
        public virtual User AddedBy { get; set; }

        public int WorkItemId { get; set; }
        public virtual WorkItem WorkItem { get; set; }
    }
}