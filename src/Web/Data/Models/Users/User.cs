using Microsoft.AspNetCore.Identity;

namespace Web.Data.Models.Users
{
    public class User : IdentityUser
    {
        public string TeamId { get; set; }
        public Team Team { get; set; }
    }
}
