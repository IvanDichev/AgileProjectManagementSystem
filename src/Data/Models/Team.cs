using Data.Models.Base;
using Data.Models.Users;
using System.Collections.Generic;

namespace Data.Models
{
    public class Team : BaseModel<int>
    {
        public Team()
        {
            this.Users = new HashSet<User>();
            this.Projects = new HashSet<Project>();
        }

        public ICollection<User> Users { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}
