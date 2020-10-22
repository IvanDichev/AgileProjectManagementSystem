using Shared.Enums;
using Data.Models.Users;

namespace Data.Models
{
    public class Ticket : BaseModel
    {
        public string ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public Severity Severity { get; set; }

        public string Description { get; set; }
    }
}