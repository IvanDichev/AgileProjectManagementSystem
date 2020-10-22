namespace Data.Models
{
    public class SubTask : BaseModel
    {
        public string Description { get; set; }

        public string TaskId { get; set; }
        public virtual Task Task { get; set; }
    }
}