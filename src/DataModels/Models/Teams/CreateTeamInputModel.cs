using System.Collections.Generic;

namespace DataModels.Models.Teams
{
    public class CreateTeamInputModel
    {
        public ICollection<int> TeamMembersIds { get; set; }
        public int? ProjectId { get; set; }
    }
}
