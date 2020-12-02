using Data.Models.Base;

namespace Data.Models
{
    public class Severity : BaseEntity<int>
    {
        public string SeverityName { get; set; }
        public int Weight { get; set; }
    }
}
