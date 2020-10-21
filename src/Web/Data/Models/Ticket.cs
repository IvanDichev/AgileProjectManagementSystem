using Shared.Enums;
using Web.Data.Models.Users;

namespace Web.Data.Models
{
    public class Ticket
    {
        public string ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public Severity Severity { get; set; }

        public string Description { get; set; }
    }
}