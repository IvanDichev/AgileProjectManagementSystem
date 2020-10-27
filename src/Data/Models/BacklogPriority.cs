using Data.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class BacklogPriority : BaseModel<int>
    {
        public string Priority { get; set; }
        public int Weight { get; set; }
    }
}
