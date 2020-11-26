using Data.Models.Base;

namespace Data.Models
{
    public class TeamRole : BaseEntity<int>
    {
        public string Role { get; set; }
    }
}
