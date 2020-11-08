using Data.Models;
using System.Collections.Generic;

namespace DataModels.Models.Teams
{
    public class CreateTeamInputModel
    {
        public string Name { get; set; }

        public ICollection<TeamsUsers> TeamsUsers { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}
