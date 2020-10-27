using Data.Models.Base;

namespace Data.Models
{
    public class SubTask : BaseModel<int>
    {
        public string Description { get; set; }

        public int TaskId { get; set; }
        public virtual Task Task { get; set; }
    }
}