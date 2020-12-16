using Data.Models.Users;
using DataModels.Models.Users;
using DataModels.Models.Users.Dtos;
using System.Collections.Generic;

namespace DataModels.Models.Projects
{
    public class ProjectViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ShortDescription => 
            this.Description?.Length > 150 ? this.Description.Substring(0, 150) + "..." : this.Description;

        public ICollection<UserDto> Users { get; set; } 

        public ICollection<UsersDropdown> UsersDropdown { get; set; }
    }
}
