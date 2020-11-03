using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Models.Users
{
    public class User : IdentityUser<int>
    {
        public int? TeamId { get; set; }
        public virtual Team Team { get; set; }

        public int? TeamRoleId { get; set; }
        public virtual TeamRole TeamRole { get; set; }

        [MaxLength(25)]
        public string FirstName { get; set; }

        [MaxLength(25)]
        public string MiddleName { get; set; }

        [MaxLength(25)]
        public string LastName { get; set; }

        public string Image { get; set; }

    }
}
