using Data.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Sprint : BaseEntity<int>
    {
        public Sprint()
        {
            this.UserStories = new HashSet<UserStory>();
            this.KanbanBoard = new HashSet<KanbanBoardColumn>();
        }

        [MaxLength(75)]
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public int StatusId { get; set; }

        public virtual SprintStatus Status { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public virtual ICollection<UserStory> UserStories { get; set; }

        public virtual ICollection<KanbanBoardColumn> KanbanBoard { get; set; }
    }
}