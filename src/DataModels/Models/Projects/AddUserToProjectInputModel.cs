using Data.Models;
using DataModels.Models.TeamRoles;
using DataModels.Models.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.Models.Projects
{
    public class AddUserToProjectInputModel
    {
        [Display(Name = "User")]
        public int UserId { get; set; }

        public int ProjectId { get; set; }

        public ICollection<UsersDropdown> UsersDropdown { get; set; }
    }
}
