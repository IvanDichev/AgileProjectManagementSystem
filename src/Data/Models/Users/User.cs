using Microsoft.AspNetCore.Identity;

namespace Data.Models.Users
{
    public class User : IdentityUser<int>
    {
        public string TeamId { get; set; }
        public Team Team { get; set; }


        public int TeamRoleId { get; set; }
        public TeamRole TeamRole { get; set; }
    }
}
