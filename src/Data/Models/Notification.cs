using Data.Models.Base;
using Data.Models.Users;

namespace Data.Models
{
    public class Notification : BaseEntity<int>
    {
        public string Message { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
