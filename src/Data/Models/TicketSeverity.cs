using Data.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class TicketSeverity : BaseModel<int>
    {
        public string Severity { get; set; }
        public int Weight { get; set; }
    }
}
