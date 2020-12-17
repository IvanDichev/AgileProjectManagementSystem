using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models.Users
{
    public class User : IdentityUser<int>
    {
        public User()
        {
            this.TeamsUsers = new HashSet<TeamsUsers>();
            this.Notifications = new HashSet<Notification>();
        }

        [MaxLength(25)]
        public string FirstName { get; set; }

        [MaxLength(25)]
        public string MiddleName { get; set; }

        [MaxLength(25)]
        public string LastName { get; set; }

        public string Image { get; set; }

        public ICollection<TeamsUsers> TeamsUsers { get; set; }

        public int? TeamRoleId { get; set; }

        public virtual TeamRole TeamRole { get; set; }

        public bool IsPublic { get; set; }

        public ICollection<Notification> Notifications { get; set; }
    }
}
