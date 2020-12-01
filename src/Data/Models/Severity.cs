using Data.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Severity : BaseEntity<int>
    {
        public string SeverityName { get; set; }
        public int Weight { get; set; }
    }
}
