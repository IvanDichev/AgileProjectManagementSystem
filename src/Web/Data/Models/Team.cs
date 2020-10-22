using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Data.Models.Users;

namespace Web.Data.Models
{
    public class Team : BaseModel
    {
        public Team()
        {
            this.Users = new HashSet<User>();
        }

        public ICollection<User> Users { get; set; }

        public TeamRoles TeamRole { get; set; }

        public string ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
