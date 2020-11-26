using Data.Models.Base;

namespace Data.Models
{
    public class SubTask : BaseEntity<int>
    {
        public string Description { get; set; }

        public int TaskId { get; set; }
        public virtual Assignment Task { get; set; }
    }
}