using Microsoft.AspNetCore.Identity;

namespace Data.Models.Users
{
    public class User : IdentityUser<int>
    {
        public string TeamId { get; set; }
        public virtual Team Team { get; set; }

        public int? TeamRoleId { get; set; }
        public virtual TeamRole TeamRole { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Image { get; set; }

    }
}
