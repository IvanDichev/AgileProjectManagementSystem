using Data.Models.Base;

namespace Data.Models
{
    public class SprintStatus : BaseEntity<int>
    {
        public string Status { get; set; }
    }
}
