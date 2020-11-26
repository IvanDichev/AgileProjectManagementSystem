using Data.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Team : BaseEntity<int>
    {
        public Team()
        {
            this.TeamsUsers = new HashSet<TeamsUsers>();
        }

        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<TeamsUsers> TeamsUsers { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}
