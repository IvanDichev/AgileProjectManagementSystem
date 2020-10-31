using Data.Models.Base;

namespace Data.Models
{
    public class SprintStatus : BaseModel<int>
    {
        public string Status { get; set; }
    }
}
