using Data.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Project : BaseEntity<int>
    {
        public Project()
        {
            this.Tickets = new HashSet<Ticket>();
            this.Sprints = new HashSet<Sprint>();
        }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public ICollection<Sprint> Sprints { get; set; }

        public virtual Team Team { get; set; }
    }
}
