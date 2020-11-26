using Data.Models.Base;

namespace Data.Models
{
    public class MockupAttachment : BaseEntity<int>
    {
        public string Attachment { get; set; }
        public int MockupId { get; set; }
        public virtual Mockup Mockup { get; set; }
    }
}
