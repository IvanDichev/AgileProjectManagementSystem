using System.Collections.Generic;
using Data.Models.Users;

namespace Data.Models
{
    public class Task : BaseModel
    {
        public Task()
        {
            //this.Requirements = new HashSet<Requirement>();
            this.SubTasks = new HashSet<SubTask>();
        }
        //public virtual ICollection<Requirement> Requirements { get; set; }

        public string Name { get; set; }

        public int Effort { get; set; }

        public ICollection<SubTask> SubTasks { get; set; }

        public string SprintId { get; set; }
        public virtual Sprint Sprint { get; set; }

        public string BacklogId { get; set; }
        public virtual UserStory Backlog { get; set; }

        public string UserId { get; set; }
        public virtual User CreatedBy { get; set; }
    }
}