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
        }

        public ICollection<User> Users { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
