using DataModels.Models.Users;
using System.Collections.Generic;

namespace DataModels.Models.Projects
{
    public class AddUserToProjectInputModel
    {
        public int UserId { get; set; }

        public int ProjectId { get; set; }

        public ICollection<UsersDropdown> UsersDropdown { get; set; }
    }
}
