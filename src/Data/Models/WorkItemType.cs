using Data.Models.Base;

namespace Data.Models
{
    public class WorkItemType : BaseEntity<int>
    {
        public string Type { get; set; }
    }
}
