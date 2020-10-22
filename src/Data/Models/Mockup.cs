namespace Data.Models
{
    public class Mockup : BaseModel
    {
        public string Attachment { get; set; }

        public string BacklogId { get; set; }
        public virtual UserStory Backlog { get; set; }
    }
}