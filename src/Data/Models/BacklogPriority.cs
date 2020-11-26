using Data.Models.Base;

namespace Data.Models
{
    public class BacklogPriority : BaseEntity<int>
    {
        public string Priority { get; set; }
        public int Weight { get; set; }
    }
}
