﻿using Shared.Enums;
using Data.Models.Users;
using Data.Models.Base;

namespace Data.Models
{
    public class Ticket : BaseEntity<int>
    {
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public int UserId { get; set; }
        public virtual User AssignedTo { get; set; }

        public int SeverityId { get; set; }
        public virtual Severity Severity { get; set; }

        public string Description { get; set; }
    }
}