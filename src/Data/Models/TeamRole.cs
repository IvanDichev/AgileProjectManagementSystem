using Data.Models.Base;

namespace Data.Models
{
    public class TeamRole : BaseModel<int>
    {
        public string Role { get; set; }
    }
}
