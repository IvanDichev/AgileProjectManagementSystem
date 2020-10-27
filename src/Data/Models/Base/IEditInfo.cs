using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models.Base
{
    public interface IEditInfo
    {
        DateTime AddedOn { get; set; }

        DateTime? ModifiedOn { get; set; }
    }
}
