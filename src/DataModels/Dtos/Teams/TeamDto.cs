using System.Collections.Generic;

namespace DataModels.Dtos.Teams
{
    public class TeamDto
    {
        public int Id { get; set; }
        public ICollection<int> TeamMembersIds { get; set; }
        public int? ProjectId { get; set; }
    }
}
