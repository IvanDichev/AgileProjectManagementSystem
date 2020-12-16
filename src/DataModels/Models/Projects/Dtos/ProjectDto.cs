using DataModels.Models.Users.Dtos;
using System.Collections.Generic;

namespace DataModels.Models.Projects.Dtos
{
    public class ProjectDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<UserDto> Users { get; set; }
    }
}
