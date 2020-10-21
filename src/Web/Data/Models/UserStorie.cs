using System.Collections.Generic;

namespace Web.Data.Models
{
    public class UserStorie : BaseModel
    {
        public UserStorie()
        {
            this.Requirements = new HashSet<Requirement>();
        }
        public virtual ICollection<Requirement> Requirements { get; set; }

        public string BacklogId { get; set; }
        public virtual Backlog Backlog { get; set; }
    }
}