using System.Collections.Generic;

namespace Web.Models.Team
{
    public class CreateTeamInputModel
    {
        public ICollection<int> TeamMembersIds { get; set; }
        public int? ProjectId { get; set; }
    }
}
