using Data.Models.Users;

namespace Data.Models
{
    public class TeamsUsers
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
    }
}
