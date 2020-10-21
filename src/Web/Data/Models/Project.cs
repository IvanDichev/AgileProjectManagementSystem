using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Web.Data.Models.Users;

namespace Web.Data.Models
{
    public class Project : BaseModel
    {
        public Project()
        {
            this.Users = new HashSet<UserProject>();
            this.Tickets = new HashSet<Ticket>();
            this.Sprints = new HashSet<Sprint>();
        }

        public DateTime DueDate { get; set; }

        public string Status { get; set; }

        public virtual ICollection<UserProject> Users { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public ICollection<Sprint> Sprints { get; set; }

        public string BacklogId { get; set; }
        public virtual Backlog Backlog { get; set; }
    }
}
